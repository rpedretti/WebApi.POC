namespace WebApi.POC.Domain
{
    /// <summary>
    /// Model for a crypto key
    /// </summary>
    public class CryptoKey
    {
        /// <summary>
        /// Id of the key
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The key kind id
        /// </summary>
        public int KindId { get; set; }

        /// <summary>
        /// The Key Kind
        /// </summary>
        public virtual KeyKind Kind { get; set; }

        /// <summary>
        /// Key Value
        /// </summary>
        public string Value { get; set; }
    }
}
