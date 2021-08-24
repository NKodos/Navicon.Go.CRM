using System;
using Microsoft.Xrm.Sdk;

namespace Navicon.Plugins
{
    public abstract class PluginBase : IPlugin
    {
        public abstract void Execute(IServiceProvider serviceProvider);

        protected ServiceInfo<T> GetBaseInfo<T>(IServiceProvider serviceProvider) where T : Entity
        {
            var info = new ServiceInfo<T>
            {
                TracingService = GetTracingService(serviceProvider)
            };

            info.TracingService.Trace("Получен Tracing Service");

            info.OrganizationService = GetOrganizationService(serviceProvider);
            info.TracingService.Trace("Получен Organization Service");

            info.TargetEntity = GetTargetEntity<T>(serviceProvider);
            info.TracingService.Trace($"Получен Target Entity типа: {nameof(T)}");

            return info;
        }

        protected IOrganizationService GetOrganizationService(IServiceProvider serviceProvider)
        {
            var serviceFactory = (IOrganizationServiceFactory)
                serviceProvider.GetService(typeof(IOrganizationServiceFactory));

            return serviceFactory.CreateOrganizationService(Guid.Empty);
        }

        protected T GetTargetEntity<T>(IServiceProvider serviceProvider) where T : Entity
        {
            var pluginContext = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            var targetEntity = (Entity)pluginContext.InputParameters["Target"];

            return targetEntity.ToEntity<T>();
        }

        protected ITracingService GetTracingService(IServiceProvider serviceProvider)
        {
            return (ITracingService)serviceProvider.GetService(typeof(ITracingService));
        }
    }
}