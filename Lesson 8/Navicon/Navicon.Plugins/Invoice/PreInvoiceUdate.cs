using Navicon.Common.Entities;
using Navicon.Plugins.Invoice.Handlers;

namespace Navicon.Plugins.Invoice
{
    public sealed class PreInvoiceUpdate : PluginBase<new_invoice>
    {
        public override void ExecuteBusinessLogics(ServiceInfo<new_invoice> serviceInfo)
        {
            var invoiceService = new InvoiceService(serviceInfo.OrganizationService);
            invoiceService.UpdateAgreementPaidAmount(serviceInfo.TargetEntity);
            invoiceService.CheckAgreementPaidAmount(serviceInfo.TargetEntity);
        }
    }
}