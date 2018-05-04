using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Shared.Domain;

namespace WebApi.POC.Repository.Local.Mapping
{
    public class RoleMap : ClassMap<Role>
    {
        public RoleMap()
        {
            Table("roles");
            Id(u => u.Id).GeneratedBy.Assigned();
            Map(u => u.Name);
        }
    }
}
