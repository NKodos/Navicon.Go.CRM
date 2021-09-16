using Microsoft.Xrm.Sdk;
using Navicon.Common.Entities;

namespace Navicon.Plugins.Invoice.Handlers
{
    public class PostInvoiceCreationService : InvoiceService
    {
        public PostInvoiceCreationService(IOrganizationService service) : base(service)
        {
        }

        public override void Execute(new_invoice invoice)
        {
            SetPayDate(invoice);
        }
    }
}