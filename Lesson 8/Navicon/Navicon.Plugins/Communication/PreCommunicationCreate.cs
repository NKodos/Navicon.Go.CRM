using System;
using Microsoft.Xrm.Sdk;
using Navicon.Common.Entities;
using Navicon.Plugins.Communication.Handler;

namespace Navicon.Plugins.Communication
{
    public sealed class PreCommunicationCreate : PluginBase
    {
        public override void Execute(IServiceProvider serviceProvider)
        {
            var serviceInfo = GetBaseInfo<new_communication>(serviceProvider);

            try
            {
                var service = new CommunicationService(serviceInfo.OrganizationService);
                service.CheckNewCommunicationMain(serviceInfo.TargetEntity);
            }
            catch (Exception ex)
            {
                serviceInfo.TracingService.Trace(ex.ToString());
                throw new InvalidPluginExecutionException(ex.Message);
            }
        }
    }
}