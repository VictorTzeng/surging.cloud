using Surging.Core.CPlatform.Exceptions;
using Surging.Core.CPlatform.Messages;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Surging.Core.KestrelHttpServer.Filters.Implementation
{
   public class CustomerExceptionFilterAttribute : IExceptionFilter
    {
        public Task OnException(ExceptionContext context)
        {
            context.Result = new HttpResultMessage<object>
            {
                Entity = null,
                StatusCode = context.Exception.GetGetExceptionStatusCode(),
                IsSucceed = false,
                Message = context.Exception.Message
            };
            return Task.CompletedTask;
        }
    }
}
