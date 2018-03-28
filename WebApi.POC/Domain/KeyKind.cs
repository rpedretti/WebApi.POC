using System.Collections.Generic;
using WebApi.Shared.Domain;

namespace WebApi.POC.Domain
{
    public sealed class KeyKind : Enumeration
    {
        public static KeyKind PUBLIC = new KeyKind(1, "PUBLIC");
        public static KeyKind PRIVATE = new KeyKind(2, "PRIVATE");

        private KeyKind() { }

        public KeyKind(int id, string name) : base(id, name) { }

        public static IEnumerable<KeyKind> List()
        {
            return new[] { PUBLIC, PRIVATE };
        }
    }
}
