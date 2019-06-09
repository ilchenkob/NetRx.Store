namespace NetRx.Store.Monitor.Shared.Models
{
    public class Message
    {
        public string StateTypeName { get; set; }

        public string ActionTypeName { get; set; }

        public string StateValueJson { get; set; }
    }
}
