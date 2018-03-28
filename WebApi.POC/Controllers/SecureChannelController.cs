using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using WebApi.POC.Services;
using WebApi.Security;
using WebApi.Shared;
using WebApi.Shared.Models;

namespace WebApi.POC.Controllers
{
    [Route("api/[controller]")]
    public class SecureChannelController : Controller
    {
        private const string _rsaKeyPath = "./pub_keys";
        private ILogger _logger;

        public SecureChannelController(ILogger<SecureChannelController> logger)
        {
            _logger = logger;
        }

        [Route("exchangepublickey")]
        public async Task<IActionResult> ExchangePublicKeys(
            [FromBody] ExchangePublicKeyModel exchangePublicKeyModel,
            [FromServices] ISecurityService securityService)
        {
            var key = await securityService.GetPublicRSAKeyAsync(0);
            await securityService.SaveClientRSAKeyAsync(exchangePublicKeyModel.Id, exchangePublicKeyModel.Key);

            return Json(new ExchangePublicKeyModel { Id = 0, Key = key });
        }

        [Route("exchangetripledeskey")]
        public async Task<IActionResult> ExchangeTripleDesKeys(
            [FromBody] ExchangePublicKeyModel exchangePublicKeyModel,
            [FromServices] ISecurityService securityService,
            [FromServices] ICryptoService cryptoService,
            [FromServices] IKeyStorageContainer storageContainer)
        {
            var rsaKey = await securityService.GetPrivateRSAKeyAsync(0);
            var clientRsaKey = await securityService.GetClientPublicRSAKeysAsync(exchangePublicKeyModel.Id);

            var encryptedClientTripleDesKey = Convert.FromBase64String(exchangePublicKeyModel.Key);
            var decryptedClientTripleDesKey = await cryptoService.DecryptRSAAsync(encryptedClientTripleDesKey, rsaKey);

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