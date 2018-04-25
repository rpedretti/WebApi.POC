using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Shared.Models
{
    public class ExchangePublicKeyModel
    {
        [JsonProperty(PropertyName = "id" )]
        [Required]
        [Display(Name = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "key")]
        [Required]
        [Display(Name = "key")]
        public string Key { get; set; }
    }
}
