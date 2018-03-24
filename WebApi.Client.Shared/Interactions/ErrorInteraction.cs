using System;
using System.Collections.Generic;
using System.Text;

namespace WebApi.Client.Shared.Interactions
{
    public sealed class StatusInteraction
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public int? Code { get; set; }
    }
}
