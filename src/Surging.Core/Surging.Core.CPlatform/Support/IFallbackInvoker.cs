using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Surging.Core.CPlatform.Support
{
    public interface IFallbackInvoker
    {
        Task Invoke(IDictionary<string, object> parameters, string serviceId, string _serviceKey);

        Task<T> Invoke<T>(IDictionary<string, object> parameters, string serviceId, string _serviceKey);

        Task<object> Invoke(IDictionary<string, object> parameters,Type returnType, string serviceId, string _serviceKey);
    }
}
