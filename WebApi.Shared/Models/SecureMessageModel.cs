using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Shared.Models
{
    public sealed class SecureMessageModel
    {
        [JsonProperty(PropertyName = "id")]
        [Required]
        [Display(Name = "id")]
        public int FromId { get; set; }

        [JsonProperty(PropertyName = "message")]
        [Required]
        [Display(Name = "message")]
        public string Message { get; set; }
    }
}
