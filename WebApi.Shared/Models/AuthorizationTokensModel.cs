namespace WebApi.Shared.Models
{
    /// <summary>
    /// Represents a Authorization Token
    /// </summary>
    public sealed class AuthorizationTokensModel
    {
        /// <summary>
        /// Gets or sets the access token.
        /// </summary>
        /// <value>
        /// The access token.
        /// </value>
        public TokenModel AccessToken { get; set; }
        /// <summary>
        /// Gets or sets the refresh token.
        /// </summary>
        /// <value>
        /// The refresh token.
        /// </value>
        public TokenModel RefreshToken { get; set; }
    }
}
