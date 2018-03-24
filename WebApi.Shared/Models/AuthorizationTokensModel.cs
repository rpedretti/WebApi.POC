namespace WebApi.Shared.Models
{
    public sealed class AuthorizationTokensModel
    {
        public TokenModel AccessToken { get; set; }
        public TokenModel RefreshToken { get; set; }
    }
}
