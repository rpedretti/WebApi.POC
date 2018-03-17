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

        public async Task<ExchangePublicKeyModel> ExchangeTripleDesKey(string key)
        {
            var payload = new ExchangePublicKeyModel()
            {
                Id = 1,
                Key = key
            };

            var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");

            var serverKeyResponse = await _httpClient.PostAsync("securechannel/exchangetripledeskey", content);

            var keyModelString = await serverKeyResponse.Content.ReadAsStringAsync();
            var keyModel = JsonConvert.DeserializeObject<ExchangePublicKeyModel>(keyModelString);

            return keyModel;
        }
    }
}
