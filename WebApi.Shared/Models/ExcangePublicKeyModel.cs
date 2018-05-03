using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Shared.Models
{
    /// <summary>
    /// Class to represent a exchange key model
    /// </summary>
    public class ExchangePublicKeyModel
    {
        /// <summary>
        /// Key owner Id
        /// </summary>
        [JsonProperty(PropertyName = "id" )]
        [Required]
        [Display(Name = "id")]
        public string Id { get; set; }

        /// <summary>
        /// Public key as string
        /// </summary>
        [JsonProperty(PropertyName = "key")]
        [Required]
        [Display(Name = "key")]
        public string Key { get; set; }
    }
}
