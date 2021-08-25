using System;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Navicon.Common.Entities;

namespace Navicon.Plugins
{
    public class ServiceInfo<T> where T : Entity
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