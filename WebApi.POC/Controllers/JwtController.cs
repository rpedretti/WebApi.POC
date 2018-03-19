using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using WebApi.Security;
using WebApi.Shared.Models;

namespace WebApi.POC.Controllers
{
    [Route("api/[controller]")]
    public class JwtController : Controller
    {
        private static Dictionary<string, string> mock = new Dictionary<string, string>()
        {
            { "fulano", "55ED885708721EDD3B5575988EFC21103F4194D18F685D0F76147E26E1E17CE3" }
        };

        [HttpPost, AllowAnonymous, Route("requestjwt")]
        public async Task<IActionResult> GetJwt([FromBody] SecureAuthenticationModel model, [FromServices] ICryptoService cryptoService)
        {
            if (ModelState.IsValid)
            {
                var key = cryptoService.RetrieveMergedKey(model.Id);
                var decryptedContent = await cryptoService.DecryptTripleDESAsync(Convert.FromBase64String(model.Content), key);

                var userModel = JsonConvert.DeserializeObject<UserAuthenticationModel>(decryptedContent);

                if (mock.TryGetValue(userModel.Username, out string password) && userModel.Password == password)
                {
                    var tokenString = BuildJwt(userModel);
                    return Json(new { id = 0, message = tokenString });
                }
                else
                {
                    return Unauthorized();
                }
            }
            else
            {
                return BadRequest(ModelState.ValidationState);
            }
        }

        private object BuildJwt(UserAuthenticationModel userModel)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SomeSecureRandomKey"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Sub, userModel.Username),
                new Claim(ClaimTypes.Role, "User"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken("http://localhost:1234",
              "myClient",
              claims,
              expires: DateTime.Now.AddMinutes(30),
              signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}