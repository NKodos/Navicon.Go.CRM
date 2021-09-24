using System;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace Navicon.Plugins
{
    public interface IServiceInfo<T> where T : Entity
    {
        IServiceProvider ServiceProvider { get; }
        ITracingService TracingService { get; }
        IOrganizationService OrganizationService { get; }
        T TargetEntity { get; set; }
        void GetTracingService();
        void GetOrganizationService();
        void GetTargetEntity();
    }

    public class ServiceInfo<T> : IServiceInfo<T> where T : Entity
    {
        public IServiceProvider ServiceProvider { get; private set; }

        public ITracingService TracingService { get; private set; }

        public IOrganizationService OrganizationService { get; private set; }

        public T TargetEntity { get; set; }

        public ServiceInfo(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        public void GetTracingService()
        {
            TracingService = (ITracingService)ServiceProvider.GetService(typeof(ITracingService));
        }

        public void GetOrganizationService()
        {
            var serviceFactory = (IOrganizationServiceFactory)
                ServiceProvider.GetService(typeof(IOrganizationServiceFactory));

            OrganizationService = serviceFactory.CreateOrganizationService(Guid.Empty);
        }

        public void GetTargetEntity()
        {
            var pluginContext = (IPluginExecutionContext)ServiceProvider.GetService(typeof(IPluginExecutionContext));

            if (pluginContext.MessageName == "Delete")
            {
                var targetEntityRef = (EntityReference)pluginContext.InputParameters["Target"];
                TargetEntity = OrganizationService.Retrieve(targetEntityRef.LogicalName, targetEntityRef.Id,
                    new ColumnSet(true)).ToEntity<T>();
            }
            else
            {
                var targetEntity = (Entity)pluginContext.InputParameters["Target"];
                TargetEntity = targetEntity.ToEntity<T>();
            }
        }
    }
}