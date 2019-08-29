using System;

namespace Surging.Core.CPlatform.Exceptions
{
    public class UnAuthorizedException : AuthException
    {
        public UnAuthorizedException(string message, StatusCode status = StatusCode.UnAuthorized) : base(message, status)
        {
        }
    }
}
