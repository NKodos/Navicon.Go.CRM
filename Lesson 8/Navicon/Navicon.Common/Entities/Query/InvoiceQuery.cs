using Microsoft.Xrm.Sdk;

namespace Navicon.Common.Entities.Query
{
    public class InvoiceQuery : EntityQuery<new_invoice>
    {
        public InvoiceQuery(IOrganizationService service) : base(service, new_invoice.EntityLogicalName)
        {
        }
    }
}