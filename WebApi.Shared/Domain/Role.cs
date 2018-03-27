using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Shared.Domain
{
    public class Role : Enumeration
    {
        public static Role USER = new Role(1, "USER");
        public static Role ADMIN = new Role(2, "ADMIN");

        protected Role() { }

        public Role(int id, string name) : base(id, name) { }

        public static IEnumerable<Role> List()
        {
            return new[] { USER, ADMIN };
        }
    }
}
