using System.Collections.Generic;

namespace WebApi.Shared.Domain
{
    /// <summary>
    /// Represents the Status enumeration
    /// </summary>
    /// <seealso cref="WebApi.Shared.Domain.Enumeration" />
    public class Status : Enumeration
    {
        /// <summary>
        /// The 'created' status
        /// </summary>
        public static Status CREATED = new Status(1, "CREATED");

        /// <summary>
        /// The 'in analisys' status
        /// </summary>
        public static Status IN_ANALISYS = new Status(2, "IN_ANALISYS");

        /// <summary>
        /// The 'in progress' status
        /// </summary>
        public static Status IN_PROGRESS = new Status(3, "IN_PROGRESS");

        /// <summary>
        /// The 'canceled' status
        /// </summary>
        public static Status CANCELED = new Status(4, "CANCELED");

        /// <summary>
        /// The 'done' status
        /// </summary>
        public static Status DONE = new Status(5, "DONE");

        protected Status() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Status"/> class.
        /// </summary>
        /// <param name="id">The enumeration identifier.</param>
        /// <param name="name">The enumeration name.</param>
        public Status(uint id, string name) : base(id, name) { }

        /// <summary>
        /// Lists this instance.
        /// </summary>
        /// <returns>Return a list with all roles</returns>
        public static IEnumerable<Status> List()
        {
            return new[] { CREATED, IN_ANALISYS, IN_PROGRESS, CANCELED, DONE };
        }

        /// <summary>
        /// Gets the formatted name.
        /// </summary>
        /// <value>
        /// The formatted name.
        /// </value>
        public virtual string FormattedName => char.ToUpper(Name[0]) + Name.Substring(1).ToLowerInvariant().Replace("_", " ");
    }
}
