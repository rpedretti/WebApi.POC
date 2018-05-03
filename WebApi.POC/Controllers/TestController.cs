using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApi.POC.Repository;
using WebApi.Shared.Domain;

namespace WebApi.POC.Controllers
{
    /// <summary>
    /// Sample controller for testing access wrights
    /// </summary>
    [Route("api/[controller]")]
    public class TestController : Controller
    {
        private ILogger _logger;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="logger">Logger instance</param>
        public TestController(ILogger<TestController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Return a list of demands
        /// </summary>
        /// <param name="demandsRepository"></param>
        /// <returns></returns>
        [HttpGet, Authorize(Roles = "User,Admin"), Route("getDemands")]
        [ProducesResponseType(typeof(List<ServiceDemand>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> GetDemands([FromServices] IDemandsRepository demandsRepository)
        {
            var demands = await demandsRepository.GetDemandsAsync(User);
            if (demands.Any())
            {
                return Ok(demands);
            }
            else
            {
                return NoContent();
            }
        }
    }
}
