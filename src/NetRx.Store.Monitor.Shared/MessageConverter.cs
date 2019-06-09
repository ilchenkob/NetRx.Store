using NetRx.Store.Monitor.Shared.Models;

namespace NetRx.Store.Monitor.Shared
{
    public static class MessageConverter
    {
        private const string Separator = "#";

        public static string Serialize(Message message)
        {
            return $"{Constants.Message.Tag}{Separator}{message.StateTypeName}{Separator}{message.ActionTypeName}{Separator}{message.StateValueJson}";
        }

        public static Message Deserialize(string message)
        {
            // TODO: get message parts via regex
            return new Message
            {
            };
        }
    }
}
