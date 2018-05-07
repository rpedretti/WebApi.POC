using FluentNHibernate.Mapping;
using WebApi.Shared.Domain;

namespace WebApi.POC.Repository.Local.Mapping
{
    /// <summary>
    /// Mapping for <see cref="User"/>
    /// </summary>
    /// <seealso cref="ClassMap&lt;T&gt;" />
    public class UserMap : ClassMap<User>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserMap"/> class.
        /// </summary>
        public UserMap()
        {
            Table("users");
            Id(u => u.Id).GeneratedBy.Identity();
            Map(u => u.Password);
            Map(u => u.Username);
            References(u => u.Role).Cascade.None();
        }
    }
}
