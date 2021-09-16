using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Navicon.Common.Entities;

namespace Navicon.Plugins.Invoice.Handlers
{
    public class PreInvoiceUpdatingService : InvoiceService
    {
        public PreInvoiceUpdatingService(IOrganizationService service) : base(service)
        {
        }

        public override void Execute(new_invoice targetInvoice)
        {
            UpdateAgreementPaidAmount(targetInvoice);
            CheckAgreementPaidAmount(targetInvoice);
        }

        protected void UpdateAgreementPaidAmount(new_invoice targetInvoice)
        {
            var columnSet = new ColumnSet(new_invoice.Fields.new_amount,
                new_invoice.Fields.new_fact, new_invoice.Fields.new_dogovorid);
            var currentInvoice = Service.Retrieve(new_invoice.EntityLogicalName, targetInvoice.Id, columnSet)
                .ToEntity<new_invoice>();

            currentInvoice.new_amount = currentInvoice.new_amount ?? new Money(0);
            targetInvoice.new_amount = targetInvoice.new_amount ?? new Money(currentInvoice.new_amount.Value);


            var currentAmount = currentInvoice.new_amount;
            var isFactChanged = targetInvoice.new_fact != null;

            if (isFactChanged)
            {
                var newAmount = new Money(targetInvoice.new_amount.Value);

                if (!targetInvoice.new_fact.GetValueOrDefault())
                {
                    newAmount.Value = currentAmount.Value * -1;
                }

                RecalculateAgreementPaidAmount(currentInvoice.new_dogovorid.Id, newAmount);
            }
            else
            {
                var diffAmountValue = targetInvoice.new_amount.Value - currentAmount.Value;
                var isAmountChanged = diffAmountValue != 0;

                if (currentInvoice.new_fact.GetValueOrDefault() && isAmountChanged)
                {
                    RecalculateAgreementPaidAmount(currentInvoice.new_dogovorid.Id, new Money(diffAmountValue));
                }
            }
        }
    }
}