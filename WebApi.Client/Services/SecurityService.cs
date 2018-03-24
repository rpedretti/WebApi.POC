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

namespace WebApi.Client.Services
{
    public class SecurityService : ISecurityService
    {
        private const string _baseUrl = ServerConstants.SERVER_URL;
        private const string _jwtFilePath = "jwt";
        private IStorageContainer _storageContainer;
        private ICryptoService _cryptoService;
        private HttpClient _httpClient;

        public SecurityService(ICryptoService cryptoService, IStorageContainer storageContainer)
        {
            _storageContainer = storageContainer;
            _cryptoService = cryptoService;
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(_baseUrl)
            };
        }

        public async Task<Tuple<string, string>> GetRsaKey(string keyPath)
        {
            return await _cryptoService.RSAKeysExists(keyPath) ?
                await _cryptoService.GetRSAKeysFromStorage(keyPath) :
                await _cryptoService.GenerateRSAKeyPairAsync(keyPath);
        }

        public async Task<ExchangePublicKeyModel> ExchangeRsaKey(string key)
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

        public async Task<ExchangePublicKeyModel> ExchangeTripleDesKey(string key, string rsaKey)
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

        public async Task RequestJwtAsync(UserAuthenticationModel userData)
        {
            var fileExists = await _storageContainer.FileExists(_jwtFilePath);
            if (!fileExists || fileExists && string.IsNullOrEmpty(await _storageContainer.ReadFileAsStringAsync(_jwtFilePath)))
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
                else
                {
                    await _storageContainer.WriteFileAsync(_jwtFilePath, "");
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task UpdateJwtAsync(UserAuthenticationModel userData)
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

        public async Task<string> SendMessageOnSecureChannel(string message, UserAuthenticationModel userData)
        {
            string response;
            try
            {
                response = await InternalSendOnSecureChannel(message);
            }
            catch (UnauthorizedAccessException)
            {
                try
                {
                    await UpdateJwtAsync(userData);
                    response = await InternalSendOnSecureChannel(message);
                }
                catch (UnauthorizedAccessException)
                {
                    response = "Unauthorized - make login again";
                }
            }
            catch (Exception e)
            {
                response = e.Message;
            }

            return response;
        }

        private async Task<string> InternalSendOnSecureChannel(string message)
        {
            try
            {
                var token = JsonConvert.DeserializeObject<TokenModel>(await _storageContainer.ReadFileAsStringAsync(_jwtFilePath));
                if (token == null)
                {
                    throw new UnauthorizedAccessException();
                }

                var key = _cryptoService.RetrieveMergedKey(0);
                var encryptedMessage = await _cryptoService.EncryptTripleDESAsync(message, key);
                var json = new SecureMessageModel()
                {
                    FromId = 1,
                    Message = Convert.ToBase64String(encryptedMessage)
                };

                var content = new StringContent(JsonConvert.SerializeObject(json), Encoding.UTF8, "application/json");

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Token);
                var response = await _httpClient.PostAsync("api/test/sayencryptedhello", content);
                var responseString = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    var responseModel = JsonConvert.DeserializeObject<SecureMessageModel>(responseString);
                    var messageBytes = Convert.FromBase64String(responseModel.Message);

                    var decrypted = await _cryptoService.DecryptTripleDESAsync(messageBytes, key);
                    return decrypted;
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
            catch (UnauthorizedAccessException ue)
            {
                throw ue;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
    }
}
