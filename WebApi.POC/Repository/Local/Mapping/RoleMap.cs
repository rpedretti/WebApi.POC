using FluentNHibernate.Mapping;
using WebApi.Shared.Domain;

namespace WebApi.POC.Repository.Local.Mapping
{
    /// <summary>
    /// Mapping for <see cref="Role"/>
    /// </summary>
    /// <seealso cref="ClassMap&lt;T&gt;" />
    public class RoleMap : ClassMap<Role>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RoleMap"/> class.
        /// </summary>
        public RoleMap()
        {
            Table("roles");
            Id(u => u.Id).GeneratedBy.Assigned();
            Map(u => u.Name);
        }
    }
}
