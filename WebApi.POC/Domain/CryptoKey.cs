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

        /// <summary>
        /// Checks if a <see cref="CryptoKey"/> equals to another one
        /// </summary>
        /// <param name="other">The <see cref="CryptoKey"/> to compare with</param>
        /// <returns>True if both <see cref="CryptoKey"/> are equal; else false</returns>
        protected bool Equals(CryptoKey other)
        {
            if (other == null)
            {
                return false;
            }

            return Id == other.Id && Kind == other.Kind;
        }

        /// <summary>
        /// Checks if a <see cref="CryptoKey"/> equals to another one
        /// </summary>
        /// <param name="obj">The object to compare with</param>
        /// <returns>True if the object is equal to this instance; else false</returns>
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

        /// <summary>
        /// Computes a hash code for this instance
        /// </summary>
        /// <returns>A hash code</returns>
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
