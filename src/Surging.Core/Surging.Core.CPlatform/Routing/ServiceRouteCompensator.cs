using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Timers;

namespace Surging.Core.CPlatform.Routing
{
    public class ServiceRouteCompensator
    {
        private readonly Timer _timer;
        private ILogger<ServiceRouteCompensator> _logger;
        private IServiceRouteProvider _serviceRouteProvider;
        private int count = 1;

        public ServiceRouteCompensator(ILogger<ServiceRouteCompensator> logger, IServiceRouteProvider serviceRouteProvider)
        {
            _logger = logger;
            _serviceRouteProvider = serviceRouteProvider;
            _timer = new Timer();
            _timer.Enabled = true;
            
            _timer.Interval = GenerateInterval();
            _timer.Elapsed += _timer_Elapsed;
            _timer.Start();

        }

        private int GenerateInterval()
        {
            var ro = new Random();
            var interval = ro.Next(10000, 50000);
            return interval;
        }

        private void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            
            if (count <= AppConfig.ServerOptions.CompensationRegisterRoutesCount)
            {
                _serviceRouteProvider.RegisterRoutes(Math.Round(Convert.ToDecimal(Process.GetCurrentProcess().TotalProcessorTime.TotalSeconds), 2, MidpointRounding.AwayFromZero)).Wait();
                if (_logger.IsEnabled(LogLevel.Debug)) {
                    _logger.LogDebug($"第{count}次进行服务路由补偿注册");
                }
                count++;
            }
            else
            {
                _timer.Stop();
                _timer.Dispose();
            }

        }
    }
}
