using System;

namespace WebApi.Shared.Domain
{

    /// <summary>
    /// Represents a Demand
    /// </summary>
    public sealed class ServiceDemand
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the owner identifier.
        /// </summary>
        /// <value>
        /// The owner identifier.
        /// </value>
        public int OwnerId { get; set; }

        /// <summary>
        /// Gets or sets the owner.
        /// </summary>
        /// <value>
        /// The owner.
        /// </value>
        public User Owner { get; set; }

        /// <summary>
        /// Gets or sets the status identifier.
        /// </summary>
        /// <value>
        /// The status identifier.
        /// </value>
        public int StatusId { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public Status Status { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the picture path.
        /// </summary>
        /// <value>
        /// The picture path.
        /// </value>
        public string PicturePath { get; set; }

        /// <summary>
        /// Gets or sets the created at.
        /// </summary>
        /// <value>
        /// The created at.
        /// </value>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the last edit.
        /// </summary>
        /// <value>
        /// The last edit.
        /// </value>
        public DateTime LastEdit { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is private.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is private; otherwise, <c>false</c>.
        /// </value>
        public bool IsPrivate { get; set; }
    }
}
