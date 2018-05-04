using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Shared.Domain;

namespace WebApi.POC.Repository.Local.Mapping
{
    public class UserMap : ClassMap<User>
    {
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
