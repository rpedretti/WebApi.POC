using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.POC.Domain;
using WebApi.Shared.Domain;

namespace WebApi.POC.Repository.Local.Mapping
{
    public class CryptoKeyMap : ClassMap<CryptoKey>
    {
        public CryptoKeyMap()
        {
            Table("cryptokeys");
            CompositeId()
                .KeyProperty(u => u.Id)
                .KeyReference(u => u.Kind)
                .Access.Field();
            Map(u => u.Value);
        }
    }
}
