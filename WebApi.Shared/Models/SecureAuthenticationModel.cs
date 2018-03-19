using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Shared.Models
{
    public sealed class SecureAuthenticationModel
    {
        [JsonProperty(PropertyName = "id")]
        [Required]
        [Display(Name = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "content")]
        [Required]
        [Display(Name = "content")]
        public string Content { get; set; }
    }
}
