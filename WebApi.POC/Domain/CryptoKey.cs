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
        public virtual string Id { get; set; }

        /// <summary>
        /// The Key Kind
        /// </summary>
        public virtual KeyKind Kind { get; set; }

        /// <summary>
        /// Key Value
        /// </summary>
        public virtual string Value { get; set; }

        protected bool Equals(CryptoKey other)
        {
            if (other == null)
            {
                return false;
            }

            return Id == other.Id && Kind == other.Kind;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            if (obj.GetType() != typeof(CryptoKey))
            {
                return false;
            }
            return Equals((CryptoKey)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Id.GetHashCode();
                hashCode = (hashCode * 397) ^ (int)Kind.Id;
                return hashCode;
            }
        }
    }
}
