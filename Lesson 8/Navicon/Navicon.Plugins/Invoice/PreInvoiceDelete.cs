using System;
using Microsoft.Xrm.Sdk;
using Navicon.Common.Entities;
using Navicon.Plugins.Invoice.Handlers;

namespace Navicon.Plugins.Invoice
{
    public sealed class PreInvoiceDelete : PluginBase
    {
        public override void Execute(IServiceProvider serviceProvider)
        {
            var serviceInfo = GetBaseInfo<new_invoice>(serviceProvider);

            try
            {
                var invoiceService = new InvoiceService(serviceInfo.OrganizationService);
                invoiceService.SubAgreementPaidAmount(serviceInfo.TargetEntity);
            }
            catch (Exception ex)
            {
                serviceInfo.TracingService.Trace(ex.ToString());
                throw new InvalidPluginExecutionException(ex.Message);
            }
        }
    }
}