using System;
namespace NetRx.Store.Exceptions
{
    public class InvalidStatePropertyTypeException : Exception
    {
        public InvalidStatePropertyTypeException(string message) : base(message)
        {
        }
    }
}
