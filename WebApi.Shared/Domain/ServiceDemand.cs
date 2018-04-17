using System;

namespace WebApi.Shared.Domain
{
    public sealed class ServiceDemand
    {
        public long Id { get; set; }
        public int OwnerId { get; set; }
        public User Owner { get; set; }
        public int StatusId { get; set; }
        public Status Status { get; set; }
        public string Description { get; set; }
        public string PicturePath { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastEdit { get; set; }
        public bool IsPrivate { get; set; }
    }
}
