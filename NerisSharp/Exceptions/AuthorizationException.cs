using System;

namespace NerisSharp.Exceptions
{
    public class AuthorizationException : Exception
    {
        public AuthorizationException() : base("Could not login, check credentials") { }
        public AuthorizationException(string message) : base(message) { }
        public AuthorizationException(string message, Exception innerException) : base(message, innerException) { }
    }
}
