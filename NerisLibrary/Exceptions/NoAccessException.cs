using System;
using System.Collections.Generic;
using System.Text;

namespace NerisSharp.Exceptions
{
    public class NoAccessException : Exception
    {
        public NoAccessException() : base("Self Imposed NO ACCESS to any methods to change or add data on the NERIS API.")
        { }
    }
}
