using FluentNHibernate.Mapping;
using WebApi.POC.Domain;

namespace WebApi.POC.Repository.Local.Mapping
{
    /// <summary>
    /// Mapping for <see cref="CryptoKey"/>
    /// </summary>
    /// <seealso cref="ClassMap&lt;T&gt;" />
    public class CryptoKeyMap : ClassMap<CryptoKey>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CryptoKeyMap"/> class.
        /// </summary>
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
