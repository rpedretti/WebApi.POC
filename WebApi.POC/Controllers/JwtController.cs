using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using WebApi.POC.Repository;
using WebApi.Security;
using WebApi.Shared.Constants;
using WebApi.Shared.Models;

namespace WebApi.POC.Controllers
{
    [Route("api/[controller]")]
    public class JwtController : Controller
    {
        private ILogger<JwtController> _logger;
        private PocDbContext _dbContext;
        private static Dictionary<string, string> refreshTokens = new Dictionary<string, string>();

        public JwtController(ILogger<JwtController> logger, PocDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        [HttpPost, Route("requestjwt")]
        public async Task<IActionResult> GetJwt(
            [FromBody] SecureAuthenticationModel model, 
            [FromServices] ICryptoService cryptoService, 
            [FromServices] IUrlHelper urlHelper)
        {
            if (ModelState.IsValid)
            {
                var key = cryptoService.RetrieveMergedKey(model.Id);
                var decryptedContent = await cryptoService.DecryptTripleDESAsync(Convert.FromBase64String(model.Content), key);

                var userModel = JsonConvert.DeserializeObject<UserAuthenticationModel>(decryptedContent);
                var user = await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Username == userModel.Username);
                if (user != null && user.Password == userModel.Password)
                {
                    var refresh = BuildRefreshJwt(userModel);
                    refreshTokens[refresh] = userModel.Username;
                    return Json(new SecureJwtModel
                    {
                        FromId = 0,
                        TokenModel = new TokenModel
                        {
                            Token = BuildJwt(userModel),
                            RefreshToken = refresh,
                            RefreshUrl = urlHelper.Action("RefreshJwt"),
                            Expires = DateTime.Now.AddMinutes(1)
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
                            RefreshUrl = urlHelper.Action("RefreshJwt"),
                            Expires = DateTime.Now.AddMinutes(1)
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
                new Claim(ClaimTypes.Role, userModel.Username == "admin" ? "Admin" : "User"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                new JwtHeader(creds),
                new JwtPayload(claims)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string BuildRefreshJwt(UserAuthenticationModel userModel)
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