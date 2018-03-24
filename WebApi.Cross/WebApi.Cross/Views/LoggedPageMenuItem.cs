using System;

namespace WebApi.Cross.Views
{

    public class LoggedPageMenuItem
    {
        public LoggedPageMenuItem()
        {
            TargetType = typeof(LoggedPageDetail);
        }
        public int Id { get; set; }
        public string Title { get; set; }

        public Type TargetType { get; set; }
    }
}