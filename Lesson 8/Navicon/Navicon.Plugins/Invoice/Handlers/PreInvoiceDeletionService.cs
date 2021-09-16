using Microsoft.Xrm.Sdk;
using Navicon.Common.Entities;

namespace Navicon.Plugins.Invoice.Handlers
{
    public class PreInvoiceDeletionService : InvoiceService
    {
        public PreInvoiceDeletionService(IOrganizationService service) : base(service)
        {
        }

        public override void Execute(new_invoice invoice)
        {
            SubAgreementPaidAmount(invoice);
        }
    }
}