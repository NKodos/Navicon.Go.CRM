using Navicon.Common.Entities;
using Navicon.Plugins.Invoice.Handlers;

namespace Navicon.Plugins.Invoice
{
    public sealed class PreInvoiceCreate : PluginBase<new_invoice>
    {
        public override void ExecuteBusinessLogics(ServiceInfo<new_invoice> serviceInfo)
        {
            var invoiceService = new PreInvoiceCreationService(serviceInfo.OrganizationService);
            invoiceService.Execute(serviceInfo.TargetEntity);
        }
    }
}