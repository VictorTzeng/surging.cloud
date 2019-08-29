using System;
using System.Collections.Generic;
using System.Text;

namespace Surging.Core.CPlatform.Exceptions
{
    public class UnAuthenticationException : AuthException
    {
        public UnAuthenticationException(string message, StatusCode status = StatusCode.UnAuthentication) : base(message, status)
        {
        }
    }
}
