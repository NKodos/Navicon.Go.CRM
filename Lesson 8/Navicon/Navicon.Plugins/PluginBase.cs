using System;
using Microsoft.Xrm.Sdk;

namespace Navicon.Plugins
{
    public abstract class PluginBase : IPlugin
    {
        public abstract void Execute(IServiceProvider serviceProvider);

        protected ServiceInfo<T> GetBaseInfo<T>(IServiceProvider serviceProvider) where T : Entity
        {
            var info = new ServiceInfo<T>(serviceProvider);

            info.GetTracingService();
            info.TracingService.Trace("Получен Tracing Service");

            info.GetOrganizationService();
            info.TracingService.Trace("Получен Organization Service");

            info.GetTargetEntity();
            info.TracingService.Trace($"Получен Target Entity типа: {typeof(T)}");

            return info;
        }
    }
}