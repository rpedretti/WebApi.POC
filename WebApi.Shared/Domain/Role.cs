using System.Collections.Generic;

namespace WebApi.Shared.Domain
{
    public sealed class Role : Enumeration
    {
        public static Role USER = new Role(1, "USER");
        public static Role ADMIN = new Role(2, "ADMIN");

        private Role() { }

        public Role(int id, string name) : base(id, name) { }

        public static IEnumerable<Role> List()
        {
            return new[] { USER, ADMIN };
        }
    }
}
