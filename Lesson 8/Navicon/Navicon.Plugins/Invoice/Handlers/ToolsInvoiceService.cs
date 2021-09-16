using Microsoft.Xrm.Sdk;
using Navicon.Common.Entities;

namespace Navicon.Plugins.Invoice.Handlers
{
    // Todo: временный класс. Используется сервисами других сущностей. Необходимо убрать!!!
    public class ToolsInvoiceService : InvoiceService
    {
        public ToolsInvoiceService(IOrganizationService service) : base(service)
        {
        }

        public override void Execute(new_invoice invoice)
        {
            throw new System.NotImplementedException();
        }
    }
}