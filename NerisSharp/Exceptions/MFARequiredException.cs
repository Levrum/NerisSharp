using System;
using System.Collections.Generic;
using System.Text;

namespace NerisSharp.Exceptions
{
    public class MFARequiredException : Exception
    {
        public MFARequiredException() : base("MFA Challenge Required, use NerisBase.LoginChallenge() method to complete login process")
        {
        }
    }
}
