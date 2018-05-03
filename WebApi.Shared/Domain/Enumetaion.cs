using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace WebApi.Shared.Domain
{
    /// <summary>
    /// Represents an serializable enumerayion
    /// </summary>
    /// <seealso cref="System.IComparable" />
    public abstract class Enumeration : IComparable<Enumeration>
    {
        /// <summary>
        /// Gets or sets the enumeration name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the enumeration identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Enumeration"/> class.
        /// </summary>
        protected Enumeration()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Enumeration"/> class.
        /// </summary>
        /// <param name="id">The enumeration identifier.</param>
        /// <param name="name">The enumeration name.</param>
        protected Enumeration(int id, string name)
        {
            Id = id;
            Name = name;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return Name;
        }

        /// <summary>
        /// Gets all enumeration as a IEnumerable.
        /// </summary>
        /// <typeparam name="T">The Enumeration type</typeparam>
        /// <returns>An <see cref="IEnumerable"/> of <typeparamref name="T"/></returns>
        public static IEnumerable<T> GetAll<T>() where T : Enumeration, new()
        {
            var type = typeof(T);
            var fields = type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);
            foreach (var info in fields)
            {
                var instance = new T();
                if (info.GetValue(instance) is T locatedValue)
                {
                    yield return locatedValue;
                }
            }
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            var otherValue = obj as Enumeration;
            if (otherValue == null)
            {
                return false;
            }
            var typeMatches = GetType().Equals(obj.GetType());
            var valueMatches = Id.Equals(otherValue.Id);
            return typeMatches && valueMatches;
        }

        /// <summary>
        /// Compares this instance to a specified <see cref="Enumeration"/> and returns an indication
        /// of their relative values.
        /// </summary>
        /// <param name="other">The other <see cref="Enumeration"/>.</param>
        /// <returns>A signed number indicating the relative values of this instance and value.Return
        ///     Value Description Less than zero This instance is less than value. Zero This
        ///     instance is equal to value. Greater than zero This instance is greater than value.
        /// </returns>
        public int CompareTo(Enumeration other)
        {
            return Id.CompareTo(other.Id);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            var hashCode = 1460282102;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + Id.GetHashCode();
            return hashCode;
        }
    }
}
