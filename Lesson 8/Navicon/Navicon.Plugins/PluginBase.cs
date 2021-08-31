using System;
using Microsoft.Xrm.Sdk;

namespace Navicon.Plugins
{
    public abstract class PluginBase<T> : IPlugin where T: Entity
    {
        public abstract void ExecuteBusinessLogics(ServiceInfo<T> serviceInfo);

        public void Execute(IServiceProvider serviceProvider)
        {
            var serviceInfo = GetBaseInfo<T>(serviceProvider);
            try
            {
                ExecuteBusinessLogics(serviceInfo);
            }
            catch (Exception ex)
            {
                serviceInfo.TracingService.Trace(ex.ToString());
                throw new InvalidPluginExecutionException(ex.Message);
            }
        }

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