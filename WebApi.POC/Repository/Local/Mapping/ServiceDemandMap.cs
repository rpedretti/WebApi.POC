using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Shared.Domain;

namespace WebApi.POC.Repository.Local.Mapping
{
    public class ServiceDemandMap : ClassMap<ServiceDemand>
    {
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