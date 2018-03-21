using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using WebApi.Security;
using WebApi.Shared.Constants;
using WebApi.Shared.Models;

namespace WebApi.POC.Controllers
{
    [Route("api/[controller]")]
    public class JwtController : Controller
    {
        private ILogger<JwtController> _logger;
        private static Dictionary<string, string> mock = new Dictionary<string, string>()
        {
            { "fulano", "55ED885708721EDD3B5575988EFC21103F4194D18F685D0F76147E26E1E17CE3" }
        };

        private static Dictionary<string, string> refreshTokens = new Dictionary<string, string>();

        public JwtController(ILogger<JwtController> logger)
        {
            _logger = logger;
        }

        [HttpPost, Route("requestjwt")]
        public async Task<IActionResult> GetJwt([FromBody] SecureAuthenticationModel model, [FromServices] ICryptoService cryptoService, [FromServices] IUrlHelper urlHelper)
        {
            if (ModelState.IsValid)
            {
                var key = cryptoService.RetrieveMergedKey(model.Id);
                var decryptedContent = await cryptoService.DecryptTripleDESAsync(Convert.FromBase64String(model.Content), key);

                var userModel = JsonConvert.DeserializeObject<UserAuthenticationModel>(decryptedContent);

                if (mock.TryGetValue(userModel.Username, out string password) && userModel.Password == password)
                {
                    var refresh = BuildRefreshJwt(model.Id, userModel);
                    refreshTokens[refresh] = userModel.Username;
                    return Json(new SecureJwtModel
                    {
                        FromId = 0,
                        TokenModel = new TokenModel
                        {
                            Token = BuildJwt(userModel),
                            RefreshToken = refresh,
                            RefreshUrl = urlHelper.Action("RefreshJwt")
                        }
                    });
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

        [HttpPost, Authorize, Route("refreshjwt", Name = "RefreshJwt")]
        public async Task<IActionResult> RefreshJwt([FromBody] SecureAuthenticationModel model, [FromServices] ICryptoService cryptoService, [FromServices] IUrlHelper urlHelper)
        {
            if (ModelState.IsValid)
            {
                var key = cryptoService.RetrieveMergedKey(model.Id);
                var refreshToken = await cryptoService.DecryptTripleDESAsync(Convert.FromBase64String(model.Content), key);

                if (refreshTokens.TryGetValue(refreshToken, out string username))
                {
                    return Json(new SecureJwtModel
                    {
                        FromId = 0,
                        TokenModel = new TokenModel
                        {
                            Token = BuildJwt(new UserAuthenticationModel { Username = username }),
                            RefreshToken = refreshToken,
                            RefreshUrl = urlHelper.Action("RefreshJwt")
                        }
                    });
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

        private string BuildJwt(UserAuthenticationModel userModel)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SomeSecureRandomKey"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Sub, userModel.Username),
                new Claim(JwtRegisteredClaimNames.Aud, "myClient"),
                new Claim(JwtRegisteredClaimNames.Iss, ServerConstants.SERVER_URL),
                new Claim(JwtRegisteredClaimNames.Nbf, new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds().ToString()),
                new Claim(JwtRegisteredClaimNames.Exp, new DateTimeOffset(DateTime.Now.AddMinutes(1)).ToUnixTimeSeconds().ToString()),
                new Claim(ClaimTypes.Role, "User"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                new JwtHeader(creds),
                new JwtPayload(claims)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string BuildRefreshJwt(int id, UserAuthenticationModel userModel)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SomeSecureRandomKey"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Sub, userModel.Username),
                new Claim(JwtRegisteredClaimNames.Aud, "myClient"),
                new Claim(JwtRegisteredClaimNames.Iss, ServerConstants.SERVER_URL),
                new Claim(JwtRegisteredClaimNames.Nbf, new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds().ToString()),
                new Claim(JwtRegisteredClaimNames.Exp, new DateTimeOffset(DateTime.Now.AddDays(5)).ToUnixTimeSeconds().ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                new JwtHeader(creds),
                new JwtPayload(claims)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}