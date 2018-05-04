using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.POC.Domain;
using WebApi.Shared.Domain;

namespace WebApi.POC.Repository.Local.Mapping
{
    public class KeyKindMap : ClassMap<KeyKind>
    {
        public KeyKindMap()
        {
            Table("keykind");
            Id(u => u.Id).GeneratedBy.Assigned();
            Map(u => u.Name);
        }
    }
}
