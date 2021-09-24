using Microsoft.Xrm.Sdk;
using Navicon.Common.Entities;
using Navicon.Plugins.Invoice.Handlers.Tools;

namespace Navicon.Plugins.Invoice.Handlers
{
    public class PostInvoiceCreationService : InvoiceService
    {
        private readonly IPayDateTool _payDateTool;

        public PostInvoiceCreationService(IOrganizationService service,
            IPayDateTool payDateTool) : base(service)
        {
            _payDateTool = payDateTool;
        }

        public override void Execute(new_invoice invoice)
        {
            UpdateDefaultPayDate(invoice);
        }
        
        private void UpdateDefaultPayDate(new_invoice targetInvoice)
        {
            var invoiceResultWithDefaultPayDate = _payDateTool.SetDefaultPayDate(targetInvoice);

            if (invoiceResultWithDefaultPayDate.Success)
            {
                Service.Update(invoiceResultWithDefaultPayDate.Value);
            }
        }
    }
}