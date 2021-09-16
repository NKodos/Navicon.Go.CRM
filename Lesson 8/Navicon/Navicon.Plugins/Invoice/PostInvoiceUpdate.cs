using Navicon.Common.Entities;
using Navicon.Plugins.Invoice.Handlers;

namespace Navicon.Plugins.Invoice
{
    public sealed class PostInvoiceUpdate : PluginBase<new_invoice>
    {
        public override void ExecuteBusinessLogics(ServiceInfo<new_invoice> serviceInfo)
        {
            var service = new PostInvoiceCreationService(serviceInfo.OrganizationService);
            service.Execute(serviceInfo.TargetEntity);
        }
    }
}