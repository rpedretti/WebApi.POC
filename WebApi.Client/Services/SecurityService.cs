using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebApi.Security;
using WebApi.Shared.Models;

namespace WebApi.Client.Services
{
    public class SecurityService : ISecurityService
    {
        private const string _baseUrl = "http://localhost:1234/api/";
        private ICryptoService _cryptoService;
        private HttpClient _httpClient;

        public SecurityService(ICryptoService cryptoService)
        {
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

        public async Task<string> SendMessageOnSecureChannel(string message)
        {
            var key = _cryptoService.RetrieveMergedKey(0);
            var encryptedMessage = await _cryptoService.EncryptTripleDESAsync(message, key);
            var json = new SecureMessageModel()
            {
                FromId = 1,
                Message = Convert.ToBase64String(encryptedMessage)
            };

            var content = new StringContent(JsonConvert.SerializeObject(json), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("test/sayencryptedhello", content);
            var responseString = await response.Content.ReadAsStringAsync();
            var responseModel = JsonConvert.DeserializeObject<SecureMessageModel>(responseString);
            var messageBytes = Convert.FromBase64String(responseModel.Message);

            var decrypted = await _cryptoService.DecryptTripleDESAsync(messageBytes, key);
            return decrypted;
        }
    }
}
