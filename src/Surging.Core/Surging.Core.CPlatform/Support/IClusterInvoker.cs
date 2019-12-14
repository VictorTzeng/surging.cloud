using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Surging.Core.CPlatform.Support
{
    public interface IClusterInvoker
    {
        Task Invoke(IDictionary<string, object> parameters, string serviceId, string _serviceKey,bool decodeJOject);

        Task<T> Invoke<T>(IDictionary<string, object> parameters, string serviceId, string _serviceKey,bool decodeJOject);

        Task<object> Invoke(IDictionary<string, object> parameters, Type returnType, string serviceId, string _serviceKey, bool decodeJOject);
    }
}
