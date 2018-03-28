using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.POC.Domain
{
    public class CryptoKey
    {
        public int Id { get; set; }

        public int KindId { get; set; }
        public virtual KeyKind Kind { get; set; }
        public string Value { get; set; }
    }
}
