using System;

namespace WebApi.Shared.Domain
{

    /// <summary>
    /// Represents a Demand
    /// </summary>
    public class ServiceDemand
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public virtual ulong Id { get; set; }

        /// <summary>
        /// Gets or sets the owner.
        /// </summary>
        /// <value>
        /// The owner.
        /// </value>
        public virtual User Owner { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public virtual Status Status { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public virtual string Description { get; set; }

        /// <summary>
        /// Gets or sets the picture path.
        /// </summary>
        /// <value>
        /// The picture path.
        /// </value>
        public virtual string PicturePath { get; set; }

        /// <summary>
        /// Gets or sets the created at.
        /// </summary>
        /// <value>
        /// The created at.
        /// </value>
        public virtual DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the last edit.
        /// </summary>
        /// <value>
        /// The last edit.
        /// </value>
        public virtual DateTime LastEdit { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is private.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is private; otherwise, <c>false</c>.
        /// </value>
        public virtual bool IsPrivate { get; set; }
    }
}
