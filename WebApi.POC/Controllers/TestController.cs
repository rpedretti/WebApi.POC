using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApi.Security;
using WebApi.Shared.Models;

namespace WebApi.POC.Controllers
{
    [Route("api/[controller]")]
    public class TestController : Controller
    {
        private ILogger _logger;

        public TestController(ILogger<TestController> logger)
        {
            _logger = logger;
        }

        [HttpPost, Route("tripleDesEncryption")]
        public async Task<IActionResult> TripleDesEncryption(string raw, [FromServices] ICryptoService cryptoService)
        {
            var key = await cryptoService.GenerateTripleDESKeyAsync();

            var encrypted = await cryptoService.EncryptTripleDESAsync(raw, key);
            var decrypted = await cryptoService.DecryptTripleDESAsync(encrypted, key);

            _logger.LogDebug("olar");

            return Json(new
            {
                raw,
                encrypted,
                decrypted
            });
        }

        [HttpPost, Route("rsaEncryption")]
        public async Task<IActionResult> RsaEncryption(string raw, [FromServices] ICryptoService cryptoService)
        {
            Tuple<string, string> key;
            if (! await cryptoService.RSAKeysExists("./"))
            {
                key = await cryptoService.GenerateRSAKeyPairAsync("./");
            } else
            {
                key = await cryptoService.GetRSAKeysFromStorage("./");
            }
            var encrypted = await cryptoService.EncryptRSAAsync(raw, key.Item1);
            var decrypted = await cryptoService.DecryptRSAAsync(encrypted, key.Item2);

            return Json(new
            {
                raw,
                encrypted,
                decrypted
            });
        }

        [HttpPost, Authorize(Roles = "User"), Route("sayencryptedhello")]
        public async Task<IActionResult> SayEncryptedHelloFromUser([FromBody] SecureMessageModel messageModel, [FromServices] ICryptoService cryptoService)
        {
            var encrypted = messageModel.Message;
            var key = cryptoService.RetrieveMergedKey(messageModel.FromId);

            var decrypted = await cryptoService.DecryptTripleDESAsync(Convert.FromBase64String(encrypted), key);

            var message = $"So you said '{decrypted}'.... got it as user";

            var encryptedResponse = await cryptoService.EncryptTripleDESAsync(message, key);

            var responseModel = new SecureMessageModel()
            {
                FromId = 0,
                Message = Convert.ToBase64String(encryptedResponse)
            };

            return Json(responseModel);
        }

        [HttpPost, Authorize(Roles = "Admin"), Route("sayencryptedhelloadmin")]
        public async Task<IActionResult> SayEncryptedHelloFromAdmin([FromBody] SecureMessageModel messageModel, [FromServices] ICryptoService cryptoService)
        {
            var encrypted = messageModel.Message;
            var key = cryptoService.RetrieveMergedKey(messageModel.FromId);

            var decrypted = await cryptoService.DecryptTripleDESAsync(Convert.FromBase64String(encrypted), key);

            var message = $"So you said '{decrypted}'.... got it as a admin";

            var encryptedResponse = await cryptoService.EncryptTripleDESAsync(message, key);

            var responseModel = new SecureMessageModel()
            {
                FromId = 0,
                Message = Convert.ToBase64String(encryptedResponse)
            };

            return Json(responseModel);
        }
    }
}
