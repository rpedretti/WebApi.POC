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

        [HttpPost("exchangepublickey")]
        public async Task<IActionResult> ExchangePublicKeys(
            [FromBody] ExchangePublicKeyModel exchangePublicKeyModel,
            [FromServices] ISecurityService securityService)
        {
            var key = await securityService.GetPublicRSAKeyAsync("server");
            await securityService.SaveClientRSAKeyAsync(exchangePublicKeyModel.Id, exchangePublicKeyModel.Key);

            return Json(new ExchangePublicKeyModel { Id = "server", Key = key });
        }
        
        [HttpPost("exchangetripledeskey")]
        public async Task<IActionResult> ExchangeTripleDesKeys(
            [FromBody] ExchangePublicKeyModel exchangePublicKeyModel,
            [FromServices] ISecurityService securityService,
            [FromServices] ICryptoService cryptoService,
            [FromServices] IKeyStorageContainer storageContainer)
        {
            var rsaKey = await securityService.GetPrivateRSAKeyAsync("server");
            var clientRsaKey = await securityService.GetClientPublicRSAKeysAsync(exchangePublicKeyModel.Id);

            var encryptedClientTripleDesKey = Convert.FromBase64String(exchangePublicKeyModel.Key);
            var decryptedClientTripleDesKey = await cryptoService.DecryptRSAAsync(encryptedClientTripleDesKey, rsaKey);

            var tripleDesKey = await cryptoService.GenerateTripleDESKeyAsync();
            var mergedKey = cryptoService.GenerateCombinedTripleDesKey(tripleDesKey, Convert.FromBase64String(decryptedClientTripleDesKey));
            cryptoService.RegisterMergedKey(exchangePublicKeyModel.Id, mergedKey);

            var model = new ExchangePublicKeyModel
            {
                Id = "server",
                Key = Convert.ToBase64String(tripleDesKey)
            };

            var encryptedModel = await cryptoService.EncryptRSAAsync(JsonConvert.SerializeObject(model), clientRsaKey);

            _logger.LogInformation("merged key: " + Convert.ToBase64String(mergedKey));

            return Json(Convert.ToBase64String(encryptedModel));
        }

        [HttpDelete("closeSecureChannel/{id}")]
        public async Task<IActionResult> CloseSecureChannel(
            string id,
            [FromServices] ICryptoService cryptoService)
        {
            cryptoService.RemoveMergedKey(id);
            return await Task.FromResult(Ok());
        }
    }
}