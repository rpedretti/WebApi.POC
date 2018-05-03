﻿using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Shared.Models
{
    /// <summary>
    /// Represents a Authentication Model
    /// </summary>
    public sealed class SecureAuthenticationModel
    {

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        [JsonProperty(PropertyName = "id")]
        [Required]
        [Display(Name = "id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the content.
        /// </summary>
        /// <value>
        /// The content.
        /// </value>
        [JsonProperty(PropertyName = "content")]
        [Required]
        [Display(Name = "content")]
        public string Content { get; set; }
    }
}
