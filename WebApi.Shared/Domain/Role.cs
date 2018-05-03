using System.Collections.Generic;

namespace WebApi.Shared.Domain
{
    /// <summary>
    /// Represents a Role enumeration
    /// </summary>
    /// <seealso cref="WebApi.Shared.Domain.Enumeration" />
    public sealed class Role : Enumeration
    {
        /// <summary>
        /// The user role
        /// </summary>
        public static Role USER = new Role(1, "USER");

        /// <summary>
        /// The admin role
        /// </summary>
        public static Role ADMIN = new Role(2, "ADMIN");

        private Role() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Role"/> class.
        /// </summary>
        /// <param name="id">The enumeration identifier.</param>
        /// <param name="name">The enumeration name.</param>
        public Role(int id, string name) : base(id, name) { }

        /// <summary>
        /// Lists this instance.
        /// </summary>
        /// <returns>Return a list with all roles</returns>
        public static IEnumerable<Role> List()
        {
            return new[] { USER, ADMIN };
        }
    }
}
