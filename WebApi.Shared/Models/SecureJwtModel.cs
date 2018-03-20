using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WebApi.Shared.Models
{
    public sealed class SecureJwtModel
    {
        [JsonProperty(PropertyName = "id")]
        [Required]
        [Display(Name = "id")]
        public int FromId { get; set; }

        [JsonProperty(PropertyName = "token")]
        [Required]
        [Display(Name = "token")]
        public TokenModel TokenModel { get; set; }
    }
}
