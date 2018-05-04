using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Shared.Domain;

namespace WebApi.POC.Repository.Local.Mapping
{
    public class StatusMap : ClassMap<Status>
    {
        public StatusMap()
        {
            Table("status");
            Id(u => u.Id).GeneratedBy.Assigned();
            Map(u => u.Name);
        }
    }
}