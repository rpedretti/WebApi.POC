using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebApi.POC.Repository;
using WebApi.Security;
using WebApi.Shared.Models;

namespace WebApi.POC.Controllers
{
    [Route("api/[controller]")]
    public class TestController : Controller
    {
        private ILogger _logger;
        private PocDbContext _dbContext;

        public TestController(ILogger<TestController> logger, PocDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        [HttpPost, Authorize(Roles = "User,Admin"), Route("sayencryptedhello")]
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

        [HttpGet, Authorize(Roles = "User,Admin"), Route("getDemands")]
        public async Task<IActionResult> GetDemands()
        {
            var username = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var services = _dbContext.ServiceDemands
                    .Include(s => s.Owner)
                    .Include(s => s.Status);

            if (User.IsInRole("Admin"))
            {
                return Ok(await services.AsNoTracking().ToListAsync());
            }
            else
            {
                return Ok(services.AsNoTracking().Where(d => d.Owner.Username == username));
            }
        }
    }
}
