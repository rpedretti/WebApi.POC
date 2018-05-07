using FluentNHibernate.Mapping;
using WebApi.Shared.Domain;

namespace WebApi.POC.Repository.Local.Mapping
{
    /// <summary>
    /// Mapping for <see cref="Status"/>
    /// </summary>
    /// <seealso cref="ClassMap&lt;T&gt;" />
    public class StatusMap : ClassMap<Status>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StatusMap"/> class.
        /// </summary>
        public StatusMap()
        {
            Table("status");
            Id(u => u.Id).GeneratedBy.Assigned();
            Map(u => u.Name);
        }
    }
}