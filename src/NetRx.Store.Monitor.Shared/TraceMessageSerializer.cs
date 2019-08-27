using NetRx.Store.Monitor.Shared.Models;
using System.Linq;

namespace NetRx.Store.Monitor.Shared
{
    public static class TraceMessageSerializer
    {
        private const char Separator = '#';

        public static string Serialize(TraceMessage message)
        {
            return $"{Constants.Message.Tag}{Separator}{message.StoreTypeName}{Separator}{message.ActionTypeName}{Separator}{message.StateValueJson}";
        }

        public static TraceMessage Deserialize(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
                return null;

            var parts = message.Split(Separator);
            if (parts.Length < 4)
                return null;

            return new TraceMessage
            {
                StoreTypeName = parts[1],
                ActionTypeName = parts[2],
                StateValueJson = parts.Skip(3).Aggregate((a, b) => $"{a}{Separator}{b}")
            };
        }
    }
}
