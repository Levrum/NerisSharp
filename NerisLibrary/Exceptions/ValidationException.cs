using System;
using System.Collections.Generic;
using System.Text;

namespace NerisSharp.Exceptions
{
    public class ValidationException : Exception
    {
        public ValidationException() { }
        public ValidationException(string message) : base(message) { }
        public ValidationException(string message, Exception innerException) : base(message, innerException) { }
    }
}
