using Microsoft.Xrm.Sdk;
using Navicon.Common.Entities;
using Navicon.Plugins.Agreement.Handlers.Tools;
using Navicon.Plugins.Interfaces.HandlersTools;

namespace Navicon.Plugins.Invoice.Handlers
{
    public class PreInvoiceDeletionService : InvoiceService
    {
        private readonly IFactSummaTool _factSummaTool;

        public PreInvoiceDeletionService(IOrganizationService service,
            IFactSummaTool factSummaTool) : base(service)
        {
            _factSummaTool = factSummaTool;
        }

        public override void Execute(new_invoice invoice)
        {
            SubAgreementPaidAmount(invoice);
        }
        
        /// <summary>
        /// Вычесть сумму счета из оплаченной суммы договора
        /// </summary>
        protected void SubAgreementPaidAmount(new_invoice targetInvoice)
        {
            if (!targetInvoice.new_fact.GetValueOrDefault()) return;

            var newAmount = new Money(targetInvoice.new_amount.Value * -1);
            RecalculateAgreementPaidAmount(_factSummaTool, targetInvoice.new_dogovorid.Id, newAmount);
        }
    }
}