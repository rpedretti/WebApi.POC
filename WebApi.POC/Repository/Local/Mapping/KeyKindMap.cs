using FluentNHibernate.Mapping;
using WebApi.POC.Domain;

namespace WebApi.POC.Repository.Local.Mapping
{
    /// <summary>
    /// Mapping for <see cref="KeyKind"/>
    /// </summary>
    /// <seealso cref="ClassMap&lt;T&gt;" />
    public class KeyKindMap : ClassMap<KeyKind>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="KeyKindMap"/> class.
        /// </summary>
        public KeyKindMap()
        {
            Table("keykind");
            Id(u => u.Id).GeneratedBy.Assigned();
            Map(u => u.Name);
        }
    }
}
