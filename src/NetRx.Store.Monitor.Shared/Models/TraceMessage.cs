namespace NetRx.Store.Monitor.Shared.Models
{
    public class TraceMessage
    {
        public string StoreTypeName { get; set; }

        public string ActionTypeName { get; set; }

        public string StateValueJson { get; set; }
    }
}
