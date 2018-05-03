using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Shared.Models
{
    /// <summary>
    /// Represents a Secure Message
    /// </summary>
    public sealed class SecureMessageModel
    {
        /// <summary>
        /// Gets or sets the origin identifier.
        /// </summary>
        /// <value>
        /// The origin identifier.
        /// </value>
        [JsonProperty(PropertyName = "id")]
        [Required]
        [Display(Name = "id")]
        public string OriginId { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        [JsonProperty(PropertyName = "message")]
        [Required]
        [Display(Name = "message")]
        public string Message { get; set; }
    }
}
