using Consul;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Net.Http.Headers;
using Polly;

namespace Surging.Core.Consul.Utilitys
{
    public static class ConsulClientExtensions
    {
        public static async Task<string[]> GetChildrenAsync(this ConsulClient client, string path)
        {
            return Policy.Handle<Exception>().WaitAndRetry(5, i => TimeSpan.FromMilliseconds(5000)).Execute<string[]>(() => {
                var queryResult = client.KV.List(path).Result;
                return queryResult.Response?.Select(p => Encoding.UTF8.GetString(p.Value)).ToArray();
            });

        }

        public static async Task<byte[]> GetDataAsync(this ConsulClient client, string path)
        {
            return Policy.Handle<Exception>().WaitAndRetry(5, i => TimeSpan.FromMilliseconds(5000)).Execute<byte[]>(() => {
                var queryResult = client.KV.Get(path).Result;
                return queryResult.Response?.Value;
            });
        }
    }
}

