using System;
namespace NetRx.Store.Exceptions
{
    public class InvalidStateTypeException : Exception
    {
        public InvalidStateTypeException(string message) : base(message)
        {
        }
    }
}
