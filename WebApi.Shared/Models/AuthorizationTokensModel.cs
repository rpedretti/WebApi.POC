using System;
using System.Collections.Generic;
using System.Text;

namespace WebApi.Shared.Models
{
    public sealed class AuthorizationTokensModel
    {
        public TokenModel AccessToken { get; set; }
        public TokenModel RefreshToken { get; set; }
    }
}
