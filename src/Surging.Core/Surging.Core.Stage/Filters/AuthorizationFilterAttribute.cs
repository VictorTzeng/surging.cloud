using Microsoft.AspNetCore.Authorization;
using Surging.Core.ApiGateWay;
using Surging.Core.ApiGateWay.OAuth;
using Surging.Core.CPlatform;
using Surging.Core.CPlatform.DependencyResolution;
using Surging.Core.CPlatform.Filters.Implementation;
using Surging.Core.CPlatform.Messages;
using Surging.Core.CPlatform.Transport.Implementation;
using Surging.Core.CPlatform.Utilities;
using Surging.Core.KestrelHttpServer.Filters;
using Surging.Core.KestrelHttpServer.Filters.Implementation;
using System.Threading.Tasks;
using Autofac;
using System;
using Surging.Core.ProxyGenerator;
using System.IO;
using Surging.Core.CPlatform.Serialization;
using System.Collections.Generic;
using Surging.Core.CPlatform.Routing;

namespace Surging.Core.Stage.Filters
{
    public class AuthorizationFilterAttribute : IAuthorizationFilter
    {
        private readonly IAuthorizationServerProvider _authorizationServerProvider;
        private readonly IServiceProxyProvider _serviceProxyProvider;
        private readonly IServiceRouteProvider _serviceRouteProvider;
        private const int _order = int.MaxValue;
        public AuthorizationFilterAttribute()
        {
            _authorizationServerProvider = ServiceLocator.Current.Resolve<IAuthorizationServerProvider>();
            _serviceProxyProvider = ServiceLocator.Current.Resolve<IServiceProxyProvider>();
            _serviceRouteProvider = ServiceLocator.Current.Resolve<IServiceRouteProvider>();
        }

        public int Order { get { return _order; } }

        public async Task OnAuthorization(AuthorizationFilterContext filterContext)
        {
            var gatewayAppConfig = AppConfig.Options.ApiGetWay;
           
            if (filterContext.Route != null && filterContext.Route.ServiceDescriptor.DisableNetwork())
            {
                var actionName = filterContext.Route.ServiceDescriptor.GroupName().IsNullOrEmpty() ? filterContext.Route.ServiceDescriptor.RoutePath : filterContext.Route.ServiceDescriptor.GroupName();
                filterContext.Result = new HttpResultMessage<object> { IsSucceed = false, StatusCode = CPlatform.Exceptions.StatusCode.UnAuthorized, Message = $"{actionName}禁止被外网访问" };
            }
            else
            {
                if (filterContext.Route != null && filterContext.Route.ServiceDescriptor.EnableAuthorization())
                {
                    if (filterContext.Route.ServiceDescriptor.AuthType() == AuthorizationType.JWT.ToString())
                    {
                        var author = filterContext.Context.Request.Headers["Authorization"];
                        if (author.Count > 0)
                        {
                            var isSuccess = await _authorizationServerProvider.ValidateClientAuthentication(author);
                            if (!isSuccess)
                            {
                                filterContext.Result = new HttpResultMessage<object> { IsSucceed = false, StatusCode = CPlatform.Exceptions.StatusCode.UnAuthentication, Message = "身份凭证(Token)不合法" };
                            }
                            else
                            {
                               
                                var payload = _authorizationServerProvider.GetPayload(author);
                                RpcContext.GetContext().SetAttachment("payload", payload);
            
                                if (!gatewayAppConfig.AuthorizationRoutePath.IsNullOrEmpty())
                                {
                                    var rpcParams = new Dictionary<string, object>() {
                                        {  "serviceId", filterContext.Route.ServiceDescriptor.Id }
                                    };
                                    var authorizationRoutePath = await _serviceRouteProvider.GetRouteByPathRegex(gatewayAppConfig.AuthorizationRoutePath);
                                    if (authorizationRoutePath == null) {
                                        filterContext.Result = new HttpResultMessage<object> { IsSucceed = false, StatusCode = CPlatform.Exceptions.StatusCode.RequestError, Message = "没有找到实现接口鉴权的WebApi的路由信息" };
                                        return;
                                    }
                                    var isPermission = await _serviceProxyProvider.Invoke<bool>(rpcParams, gatewayAppConfig.AuthorizationRoutePath, gatewayAppConfig.AuthorizationServiceKey);
                                    if (!isPermission)
                                    {
                                        var actionName = filterContext.Route.ServiceDescriptor.GroupName().IsNullOrEmpty() ? filterContext.Route.ServiceDescriptor.RoutePath : filterContext.Route.ServiceDescriptor.GroupName();
                                        filterContext.Result = new HttpResultMessage<object> { IsSucceed = false, StatusCode = CPlatform.Exceptions.StatusCode.RequestError, Message = $"没有请求{actionName}的权限" };
                                    }
                                }
                               
                            }
                        }
                        else
                        {
                            filterContext.Result = new HttpResultMessage<object> { IsSucceed = false, StatusCode = CPlatform.Exceptions.StatusCode.UnAuthentication, Message = "您还没有登录系统,请先登录系统" };
                        }

                    }
                }
            }

            if (String.Compare(filterContext.Path.ToLower(), gatewayAppConfig.TokenEndpointPath, true) == 0)
            {
                filterContext.Context.Items.Add("path", gatewayAppConfig.AuthenticationRoutePath);
            }           
        }
    }
}
 
