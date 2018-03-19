using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Shared.Models
{
    public sealed class UserAuthenticationModel
    {
        [JsonProperty(PropertyName = "username")]
        [Required]
        [Display(Name = "username")]
        public string Username { get; set; }

        [JsonProperty(PropertyName = "password")]
        [Required]
        [Display(Name = "password")]
        public string Password { get; set; }
    }
}
