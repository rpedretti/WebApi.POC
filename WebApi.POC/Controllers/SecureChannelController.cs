using System;
using System.Net;
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
    /// <summary>
    /// Controls the secure channel between the client and server
    /// </summary>
    [Route("api/[controller]")]
    public class SecureChannelController : Controller
    {
        private const string _rsaKeyPath = "./pub_keys";
        private ILogger _logger;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="logger">Logger instance</param>
        public SecureChannelController(ILogger<SecureChannelController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Exchanges public RSA key with the client
        /// </summary>
        /// <param name="exchangePublicKeyModel">The client id and key</param>
        /// <param name="securityService">The securityService instance</param>
        /// <returns></returns>
        [HttpPost("exchangepublickey")]
        [ProducesResponseType(typeof(ExchangePublicKeyModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ExchangePublicKeys(
            [FromBody] ExchangePublicKeyModel exchangePublicKeyModel,
            [FromServices] ISecurityService securityService)
        {
            if (ModelState.IsValid)
            {
                var key = await securityService.GetPublicRSAKeyAsync("server");
                await securityService.SaveClientRSAKeyAsync(exchangePublicKeyModel.Id, exchangePublicKeyModel.Key);

                return Json(new ExchangePublicKeyModel { Id = "server", Key = key });
            }
            else
            {
                return BadRequest(ModelState.ValidationState);
            }
        }

        /// <summary>
        /// Exchanges a 3DES key with the client
        /// </summary>
        /// <param name="exchangePublicKeyModel">The client id and key</param>
        /// <param name="securityService">The securityService instance</param>
        /// <param name="cryptoService">The cryptoService instance</param>
        /// <param name="storageContainer">The storageContainer instance</param>
        /// <returns></returns>
        [HttpPost("exchangetripledeskey")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ExchangeTripleDesKeys(
            [FromBody] ExchangePublicKeyModel exchangePublicKeyModel,
            [FromServices] ISecurityService securityService,
            [FromServices] ICryptoService cryptoService,
            [FromServices] IKeyStorageContainer storageContainer)
        {
            if (ModelState.IsValid)
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
            else
            {
                return BadRequest(ModelState.ValidationState);
            }
        }

        /// <summary>
        /// Closes a secure channel
        /// </summary>
        /// <param name="id">The client id</param>
        /// <param name="cryptoService">The cryptoService instance</param>
        /// <returns></returns>
        [HttpDelete("closeSecureChannel/{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> CloseSecureChannel(
            string id,
            [FromServices] ICryptoService cryptoService)
        {
            if (cryptoService.RemoveMergedKey(id))
            {
                return await Task.FromResult(Ok());
            }
            else
            {
                return await Task.FromResult(NotFound());
            }
        }
    }
}