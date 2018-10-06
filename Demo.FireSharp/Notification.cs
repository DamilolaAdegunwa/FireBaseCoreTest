using System;

namespace Demo.FireSharp
{
    public class Notification
    {
        public string Channel { get; set; }
        public int Id { get; set; }
        public string Message { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreateOnText => CreatedOn.ToString("mdd/MM/yyyy HH:mm");
        public bool IsError { get; set; }
    }
}
