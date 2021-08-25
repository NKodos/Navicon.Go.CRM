using System;
using Microsoft.Xrm.Sdk;
using Navicon.Common.Entities;
using Navicon.Plugins.Agreement.Handlers;
using Navicon.Plugins.Invoice.Handlers;

namespace Navicon.Plugins.Agreement
{
    public sealed class PostAgreementCreate : PluginBase
    {
        public override void Execute(IServiceProvider serviceProvider)
        {
            var serviceInfo = GetBaseInfo<new_agreement>(serviceProvider);

            try
            {
                var service = new PostAgreementService(serviceInfo.OrganizationService);
                service.TrySetFact(serviceInfo.TargetEntity);
            }
            catch (Exception ex)
            {
                serviceInfo.TracingService.Trace(ex.ToString());
                throw new InvalidPluginExecutionException(ex.Message);
            }
        }
    }
}