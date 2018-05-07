using FluentNHibernate.Mapping;
using WebApi.Shared.Domain;

namespace WebApi.POC.Repository.Local.Mapping
{
    /// <summary>
    /// Mapping for <see cref="ServiceDemand"/>
    /// </summary>
    /// <seealso cref="ClassMap&lt;T&gt;" />
    public class ServiceDemandMap : ClassMap<ServiceDemand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceDemandMap"/> class.
        /// </summary>
        public ServiceDemandMap()
        {
            Table("services");
            Id(u => u.Id).GeneratedBy.Identity();
            Map(u => u.CreatedAt);
            Map(u => u.Description);
            Map(u => u.IsPrivate);
            Map(u => u.LastEdit);
            References(u => u.Owner).Cascade.None();
            Map(u => u.PicturePath);
            References(u => u.Status).Cascade.None();
        }
    }
}