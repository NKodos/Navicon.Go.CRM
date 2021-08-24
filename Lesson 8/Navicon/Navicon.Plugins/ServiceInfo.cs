using System;
using Microsoft.Xrm.Sdk;

namespace Navicon.Plugins
{
    public class ServiceInfo<T> where T : Entity
    {
        public ITracingService TracingService { get; set; }

        public IOrganizationService OrganizationService { get; set; }

        public T TargetEntity { get; set; }
    }
}