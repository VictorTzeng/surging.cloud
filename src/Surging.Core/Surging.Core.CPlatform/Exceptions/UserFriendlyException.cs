using System;

namespace Surging.Core.CPlatform.Exceptions
{
    public class UserFriendlyException : BusinessException
    {

        public UserFriendlyException(string message) : base(message, StatusCode.UserFriendly)
        {

        }
    }
}
