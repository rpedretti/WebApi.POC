using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using WebApi.Security;
using WebApi.Shared;
using WebApi.Shared.Models;

namespace WebApi.POC.Controllers
{
    [Route("api/[controller]")]
    public class SecureChannelController : Controller
    {
        private const string _rsaKeyPath = "./";
        private ILogger _logger;

        public SecureChannelController(ILogger<SecureChannelController> logger)
        {
            _logger = logger;
        }

        [Route("exchangepublickey")]
        public async Task<IActionResult> ExchangePublicKeys(
            [FromBody] ExchangePublicKeyModel exchangePublicKeyModel,
            [FromServices] ICryptoService cryptoService,
            [FromServices] IStorageContainer storageContainer)
        {
            var keys = await cryptoService.RSAKeysExists(_rsaKeyPath) ?
                await cryptoService.GetRSAKeysFromStorage(_rsaKeyPath) :
                await cryptoService.GenerateRSAKeyPairAsync(_rsaKeyPath);

            await storageContainer.WriteFileAsync($"./{exchangePublicKeyModel.Id}/key.pub", exchangePublicKeyModel.Key);

            return Json(new ExchangePublicKeyModel { Id = 0, Key = keys.Item1 });
        }

        [Route("exchangetripledeskey")]
        public async Task<IActionResult> ExchangeTripleDesKeys(
            [FromBody] ExchangePublicKeyModel exchangePublicKeyModel,
            [FromServices] ICryptoService cryptoService,
            [FromServices] IStorageContainer storageContainer)
        {
            var rsaKey = await cryptoService.GetRSAKeysFromStorage("");
            var clientRsaKey = await storageContainer.ReadFileAsStringAsync($"./{exchangePublicKeyModel.Id}/key.pub");

            var encryptedClientTripleDesKey = Convert.FromBase64String(exchangePublicKeyModel.Key);
            var decryptedClientTripleDesKey = await cryptoService.DecryptRSAAsync(encryptedClientTripleDesKey, rsaKey.Item2);

            var tripleDesKey = await cryptoService.GenerateTripleDESKeyAsync();
            var mergedKey = cryptoService.GenerateCombinedTripleDesKey(tripleDesKey, Convert.FromBase64String(decryptedClientTripleDesKey));
            cryptoService.RegisterMergedKey(exchangePublicKeyModel.Id, mergedKey);

            var model = new ExchangePublicKeyModel
            {
                Id = 0,
                Key = Convert.ToBase64String(tripleDesKey)
            };

            var encryptedModel = await cryptoService.EncryptRSAAsync(JsonConvert.SerializeObject(model), clientRsaKey);

            _logger.LogInformation("merged key: " + Convert.ToBase64String(mergedKey));

            return Json(Convert.ToBase64String(encryptedModel));
        }
    }
}