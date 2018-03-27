using System;
using System.Collections.Generic;
using System.Text;

namespace WebApi.Shared.Domain
{
    public class Status : Enumeration
    {
        public static Status CREATED = new Status(1, "CREATED");
        public static Status IN_ANALISYS = new Status(2, "IN_ANALISYS");
        public static Status IN_PROGRESS = new Status(3, "IN_PROGRESS");
        public static Status CANCELED = new Status(4, "CANCELED");
        public static Status DONE = new Status(5, "DONE");

        protected Status() { }

        public Status(int id, string name) : base(id, name) { }

        public static IEnumerable<Status> List()
        {
            return new[] { CREATED, IN_ANALISYS, IN_PROGRESS, CANCELED, DONE };
        }

        public string FormattedName => char.ToUpper(Name[0]) + Name.Substring(1).ToLowerInvariant().Replace("_", " ");
    }
}
