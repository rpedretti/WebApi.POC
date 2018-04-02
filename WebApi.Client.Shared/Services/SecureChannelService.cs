using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using WebApi.Security;
using WebApi.Shared;
using WebApi.Shared.Constants;
using WebApi.Shared.Models;

namespace WebApi.Client.Shared.Services
{
    public class SecureChannelService : ISecureChannelService
    {
        #region Private Properties

        private static UserAuthenticationModel authenticatedUser;
        private const string _rsaKeyPath = "";
        private const string _baseUrl = ServerConstants.SERVER_URL;
        private const string _jwtFilePath = "jwt";
        private IKeyStorageContainer _keyStorageContainer;
        private IStorageContainer _storageContainer;
        private ICryptoService _cryptoService;
        private HttpClient _httpClient;

        #endregion Private Properties

        #region Public Methods

        public SecureChannelService(ICryptoService cryptoService, IStorageContainer storageContainer, IKeyStorageContainer keyStorageContainer)
        {
            _storageContainer = storageContainer;
            _keyStorageContainer = keyStorageContainer;
            _cryptoService = cryptoService;
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(_baseUrl)
            };
        }

        public async Task RequestJwtAsync(UserAuthenticationModel userData, bool forceRefresh)
        {
            var fileExists = await _keyStorageContainer.PublicKeyExists(1);
            if (forceRefresh || !fileExists || fileExists && string.IsNullOrEmpty(await _storageContainer.ReadFileAsStringAsync(_jwtFilePath)))
            {
                var key = _cryptoService.RetrieveMergedKey(0);
                var cryptedData = await _cryptoService.EncryptTripleDESAsync(JsonConvert.SerializeObject(userData), key);

                var jwtRequest = new SecureAuthenticationModel()
                {
                    Id = 1,
                    Content = Convert.ToBase64String(cryptedData)
                };

                var content = new StringContent(JsonConvert.SerializeObject(jwtRequest), Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("api/jwt/requestjwt", content);
                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    var responseModel = JsonConvert.DeserializeObject<SecureJwtModel>(responseString);
                    await _storageContainer.WriteFileAsync(_jwtFilePath, JsonConvert.SerializeObject(responseModel.TokenModel));
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new UnauthorizedAccessException();
                }
                else
                {
                    await _storageContainer.WriteFileAsync(_jwtFilePath, "");
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task UpdateJwtAsync()
        {
            var key = _cryptoService.RetrieveMergedKey(0);
            var token = JsonConvert.DeserializeObject<TokenModel>(await _storageContainer.ReadFileAsStringAsync(_jwtFilePath));

            var cryptedData = await _cryptoService.EncryptTripleDESAsync(token.RefreshToken, key);

            var jwtRequest = new SecureAuthenticationModel()
            {
                Id = 1,
                Content = Convert.ToBase64String(cryptedData)
            };
            var content = new StringContent(JsonConvert.SerializeObject(jwtRequest), Encoding.UTF8, "application/json");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.RefreshToken);
            var response = await _httpClient.PostAsync(token.RefreshUrl, content);
            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                var responseModel = JsonConvert.DeserializeObject<SecureJwtModel>(responseString);
                await _storageContainer.WriteFileAsync(_jwtFilePath, JsonConvert.SerializeObject(responseModel.TokenModel));
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                await _storageContainer.WriteFileAsync(_jwtFilePath, "");
                throw new UnauthorizedAccessException();
            }
            else
            {
                await _storageContainer.WriteFileAsync(_jwtFilePath, "");
                throw new Exception(response.ReasonPhrase);
            }
        }

        public async Task<T> PostOnSecureChannelAsync<T>(object message, string url)
        {
            var stringMessage = message is string ? message as string : JsonConvert.SerializeObject(message);
            var response = await InternalPostOnSecureChannelAsync<T>(stringMessage, url);
            return response;
        }

        public async Task PostOnSecureChannelAsync(object message, string url)
        {
            var stringMessage = message is string ? message as string : JsonConvert.SerializeObject(message);
            await InternalPostOnSecureChannelAsync(stringMessage, url);
        }

        public async Task<T> GetOnSecureChannelAsync<T>(string url)
        {
            TokenModel token = await GetJwtTokenAsync();

            string result;

            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Token);
                result = await _httpClient.GetStringAsync(url);
            }
            catch (Exception)
            {
                result = null;
            }
            return JsonConvert.DeserializeObject<T>(result);

        }

        public async Task OpenSecureChannelAsync(string username, string password, bool forceTokenUpdate = true)
        {
            Tuple<string, string> keys = await GetRSAKeys();

            // Sends public key and get server's public key in return
            var serverRsaKey = await ExchangeRsaKeyAsync(keys.Item1);

            // Generates a 3DES key
            var tripleDesKey = await _cryptoService.GenerateTripleDESKeyAsync();

            // Encrypt the 3DES key with server RSA public key
            var encryptedTripleDesKey = await _cryptoService.EncryptRSAAsync(Convert.ToBase64String(tripleDesKey), serverRsaKey.Key);

            // Sends the encrypted key to the server and gets an 3DES key in return
            var serverTripleDesMessage = await ExchangeTripleDesKeyAsync(Convert.ToBase64String(encryptedTripleDesKey), keys.Item2);

            // Merges both 3DES key to generate a new key used by both sides
            var mergedKey = _cryptoService.GenerateCombinedTripleDesKey(tripleDesKey, Convert.FromBase64String(serverTripleDesMessage.Key));
            _cryptoService.RegisterMergedKey(serverRsaKey.Id, mergedKey);


            var userData = new UserAuthenticationModel()
            {
                Username = username,
                Password = password
            };

            await RequestJwtAsync(userData, forceTokenUpdate);
            authenticatedUser = userData;
        }

        private async Task<Tuple<string, string>> GetRSAKeys()
        {
            Tuple<string, string> keys;
            if (await RSAKeysExists(1))
            {
                keys = await GetRSAKeysFromStorage(1);
            }
            else
            {
                keys = await _cryptoService.GenerateRSAKeyPairAsync();
                await _keyStorageContainer.WritePublicKeyAsync(1, keys.Item1);
                await _keyStorageContainer.WritePrivateKeyAsync(1, keys.Item2);
            }

            return keys;
        }

        #endregion Public Methods

        #region Private Methods

        private async Task<ExchangePublicKeyModel> ExchangeRsaKeyAsync(string key)
        {
            var payload = new ExchangePublicKeyModel()
            {
                Id = 1,
                Key = key
            };

            var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");

            var serverKeyResponse = await _httpClient.PostAsync("api/securechannel/exchangepublickey", content);

            var keyModelString = await serverKeyResponse.Content.ReadAsStringAsync();
            var keyModel = JsonConvert.DeserializeObject<ExchangePublicKeyModel>(keyModelString);

            return keyModel;
        }

        private async Task<ExchangePublicKeyModel> ExchangeTripleDesKeyAsync(string key, string rsaKey)
        {
            var payload = new ExchangePublicKeyModel()
            {
                Id = 1,
                Key = key
            };

            var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");

            var serverKeyResponse = await _httpClient.PostAsync("api/securechannel/exchangetripledeskey", content);

            var keyModelJson = await serverKeyResponse.Content.ReadAsStringAsync();

            var keyModelString = JsonConvert.DeserializeObject<string>(keyModelJson);

            var decryptedServerModel = await _cryptoService.DecryptRSAAsync(Convert.FromBase64String(keyModelString), rsaKey);
            var keyModel = JsonConvert.DeserializeObject<ExchangePublicKeyModel>(decryptedServerModel);

            return keyModel;
        }

        private async Task<TokenModel> GetJwtTokenAsync()
        {
            var tokenString = await _storageContainer.ReadFileAsStringAsync(_jwtFilePath);
            var token = JsonConvert.DeserializeObject<TokenModel>(tokenString);
            if (token == null)
            {
                throw new UnauthorizedAccessException();
            }

            if (token.IsExpired)
            {
                await UpdateJwtAsync();
                tokenString = await _storageContainer.ReadFileAsStringAsync(_jwtFilePath);
                token = JsonConvert.DeserializeObject<TokenModel>(tokenString);
            }

            return token;
        }

        private async Task<T> InternalPostOnSecureChannelAsync<T>(string message, string url)
        {
            var key = _cryptoService.RetrieveMergedKey(0);
            var content = await PrepareForPostAsync(message, key);
            var response = await _httpClient.PostAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                var responseModel = JsonConvert.DeserializeObject<SecureMessageModel>(responseString);
                var messageBytes = Convert.FromBase64String(responseModel.Message);

                var decrypted = await _cryptoService.DecryptTripleDESAsync(messageBytes, key);
                var result = JsonConvert.DeserializeObject<T>(decrypted);
                return result;
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new UnauthorizedAccessException();
            }
            else
            {
                throw new Exception(response.ReasonPhrase);
            }
        }

        private async Task InternalPostOnSecureChannelAsync(string message, string url)
        {
            var key = _cryptoService.RetrieveMergedKey(0);
            var content = await PrepareForPostAsync(message, key);
            var response = await _httpClient.PostAsync(url, content);

            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new UnauthorizedAccessException();
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        private async Task<StringContent> PrepareForPostAsync(string message, byte[] encryptKey)
        {
            var token = await GetJwtTokenAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Token);

            var encryptedMessage = await _cryptoService.EncryptTripleDESAsync(message, encryptKey);
            var json = new SecureMessageModel()
            {
                FromId = 1,
                Message = Convert.ToBase64String(encryptedMessage)
            };

            var content = new StringContent(JsonConvert.SerializeObject(json), Encoding.UTF8, "application/json");

            return content;
        }

        private async Task<bool> RSAKeysExists(int id)
        {
            return await _keyStorageContainer.PublicKeyExists(id)
                && await _keyStorageContainer.PrivateKeyExists(id);
        }

        private async Task<Tuple<string, string>> GetRSAKeysFromStorage(int id)
        {
            var publicKey = await _keyStorageContainer.ReadPublickKeyAsStringAsync(id);
            var publicPrvateKey = await _keyStorageContainer.ReadPrivateKeyAsStringAsync(id);

            return Tuple.Create(publicKey, publicPrvateKey);
        }

        #endregion Private Methods
    }
}
