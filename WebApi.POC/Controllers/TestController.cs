﻿using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApi.Security;

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

        [HttpPost]
        [Route("tripleDesEncryption")]
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

        [HttpPost]
        [Route("rsaEncryption")]
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
    }
}
