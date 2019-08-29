using System;

namespace Surging.Core.CPlatform.Exceptions
{
    public class ValidateException : BusinessException
    {

        public ValidateException(string message) : base(message, StatusCode.ValidateError)
        {

        }
    }
}
