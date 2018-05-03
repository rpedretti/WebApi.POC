using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using WebApi.POC.Repository;
using WebApi.Security;
using WebApi.Shared.Constants;
using WebApi.Shared.Models;

namespace WebApi.POC.Controllers
{
    /// <summary>
    /// Class responsible for handling JWT requests
    /// </summary>
    [Route("api/[controller]")]
    public class JwtController : Controller
    {
        private ILogger<JwtController> _logger;
        private static Dictionary<string, string> refreshTokens = new Dictionary<string, string>();

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="logger">Logger instance</param>
        public JwtController(ILogger<JwtController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Requests a new JWT Token
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userRepository"></param>
        /// <param name="cryptoService"></param>
        /// <param name="urlHelper"></param>
        /// <returns></returns>
        [HttpPost, Route("requestjwt")]
        [ProducesResponseType(typeof(SecureJwtModel), 200)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetJwt(
            [FromBody] SecureAuthenticationModel model, 
            [FromServices] IUserRepository userRepository,
            [FromServices] ICryptoService cryptoService, 
            [FromServices] IUrlHelper urlHelper)
        {
            if (ModelState.IsValid)
            {
                var key = cryptoService.RetrieveMergedKey(model.Id);
                var decryptedContent = await cryptoService.DecryptTripleDESAsync(Convert.FromBase64String(model.Content), key);

                var userModel = JsonConvert.DeserializeObject<UserAuthenticationModel>(decryptedContent);
                var user = await userRepository.GetUserAsync(userModel.Username);
                if (user != null && user.Password == userModel.Password)
                {
                    var refresh = BuildRefreshJwt(userModel);
                    refreshTokens[refresh] = userModel.Username;
                    return Json(new SecureJwtModel
                    {
                        OriginId = 0,
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

        /// <summary>
        /// Refresh an existing JWT
        /// </summary>
        /// <param name="model"></param>
        /// <param name="cryptoService"></param>
        /// <param name="urlHelper"></param>
        /// <returns></returns>
        [HttpPost, Authorize, Route("refreshjwt", Name = "RefreshJwt")]
        [ProducesResponseType(typeof(SecureJwtModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> RefreshJwt(
            [FromBody] SecureAuthenticationModel model, 
            [FromServices] ICryptoService cryptoService, 
            [FromServices] IUrlHelper urlHelper)
        {
            if (ModelState.IsValid)
            {
                var key = cryptoService.RetrieveMergedKey(model.Id);
                var refreshToken = await cryptoService.DecryptTripleDESAsync(Convert.FromBase64String(model.Content), key);

                if (refreshTokens.TryGetValue(refreshToken, out string username))
                {
                    return Json(new SecureJwtModel
                    {
                        OriginId = 0,
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