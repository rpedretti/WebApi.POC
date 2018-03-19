using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using WebApi.Security;
using WebApi.Shared;
using WebApi.Shared.Models;

namespace WebApi.Client.Services
{
    public class SecurityService : ISecurityService
    {
        private const string _baseUrl = "http://localhost:1234/api/";
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

        public async Task<ExchangePublicKeyModel> ExchangeRsaKey(string key)
        {
            var payload = new ExchangePublicKeyModel()
            {
                Id = 1,
                Key = key
            };

            var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");

            var serverKeyResponse = await _httpClient.PostAsync("securechannel/exchangepublickey", content);

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

            var serverKeyResponse = await _httpClient.PostAsync("securechannel/exchangetripledeskey", content);

            var keyModelJson = await serverKeyResponse.Content.ReadAsStringAsync();

            var keyModelString = JsonConvert.DeserializeObject<string>(keyModelJson);

            var decryptedServerModel = await _cryptoService.DecryptRSAAsync(Convert.FromBase64String(keyModelString), rsaKey);
            var keyModel = JsonConvert.DeserializeObject<ExchangePublicKeyModel>(decryptedServerModel);

            return keyModel;
        }

        public async Task RequestJwtAsync(UserAuthenticationModel userData)
        {
            if (!await _storageContainer.FileExists(_jwtFilePath))
            {
                var key = _cryptoService.RetrieveMergedKey(0);
                var cryptedData = await _cryptoService.EncryptTripleDESAsync(JsonConvert.SerializeObject(userData), key);

                var jwtRequest = new SecureAuthenticationModel()
                {
                    Id = 1,
                    Content = Convert.ToBase64String(cryptedData)
                };

                var content = new StringContent(JsonConvert.SerializeObject(jwtRequest), Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("jwt/requestjwt", content);
                var responseString = await response.Content.ReadAsStringAsync();
                var responseModel = JsonConvert.DeserializeObject<SecureMessageModel>(responseString);
                await _storageContainer.WriteFileAsync(_jwtFilePath, responseModel.Message);
            }
        }

        public async Task<string> SendMessageOnSecureChannel(string message)
        {
            var token = await _storageContainer.ReadFileAsStringAsync(_jwtFilePath);
            var key = _cryptoService.RetrieveMergedKey(0);
            var encryptedMessage = await _cryptoService.EncryptTripleDESAsync(message, key);
            var json = new SecureMessageModel()
            {
                FromId = 1,
                Message = Convert.ToBase64String(encryptedMessage)
            };

            var content = new StringContent(JsonConvert.SerializeObject(json), Encoding.UTF8, "application/json");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.PostAsync("test/sayencryptedhello", content);
            var responseString = await response.Content.ReadAsStringAsync();
            var responseModel = JsonConvert.DeserializeObject<SecureMessageModel>(responseString);
            var messageBytes = Convert.FromBase64String(responseModel.Message);

            var decrypted = await _cryptoService.DecryptTripleDESAsync(messageBytes, key);
            return decrypted;
        }
    }
}
