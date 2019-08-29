using System;

namespace Surging.Core.CPlatform.Exceptions
{
    public class AuthException : BusinessException
    {
        public AuthException(string message, StatusCode status = StatusCode.UnAuthentication) : base(message, status)
        {
        }

    }
}
