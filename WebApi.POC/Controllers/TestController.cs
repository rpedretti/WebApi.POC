using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebApi.POC.Repository;

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
                return Ok(services.AsNoTracking().Where(d => d.Owner.Username == username && d.IsPrivate == false));
            }
        }
    }
}
