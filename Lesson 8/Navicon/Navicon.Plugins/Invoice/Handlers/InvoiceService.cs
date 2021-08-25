using System;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Navicon.Common.Entities;
using Navicon.Plugins.Agreement.Handlers;

namespace Navicon.Plugins.Invoice.Handlers
{
    public class InvoiceService
    {
        private readonly IOrganizationService _service;

        public InvoiceService(IOrganizationService service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        public void CheckInvoiceType(new_invoice targetInvoice)
        {
            if (targetInvoice.new_type == null)
            {
                targetInvoice.new_type = new_invoice_new_type.__100000000;
            }
        }

        public void AddAgreementPaidAmount(new_invoice targetEntity)
        {
            if (!targetEntity.new_fact.GetValueOrDefault()) return;

            RecalculateAgreementPaidAmount(targetEntity.new_dogovorid.Id, targetEntity.new_amount);
        }

        public void SubAgreementPaidAmount(new_invoice targetEntity)
        {
            if (!targetEntity.new_fact.GetValueOrDefault()) return;

            var newAmount = new Money(targetEntity.new_amount.Value * -1);
            RecalculateAgreementPaidAmount(targetEntity.new_dogovorid.Id, newAmount);
        }

        public void UpdateAgreementPaidAmount(new_invoice targetEntity)
        {
            var columnSet = new ColumnSet(new_invoice.Fields.new_amount, 
                new_invoice.Fields.new_fact, new_invoice.Fields.new_dogovorid);
            var currentInvoice = _service.Retrieve(new_invoice.EntityLogicalName, targetEntity.Id, columnSet)
                .ToEntity<new_invoice>();

            currentInvoice.new_amount = currentInvoice.new_amount ?? new Money(0);
            targetEntity.new_amount = targetEntity.new_amount ?? new Money(currentInvoice.new_amount.Value);

            var currentAmount = currentInvoice.new_amount;
            var diffAmountValue = targetEntity.new_amount.Value - currentAmount.Value;
            var isAmountChanged = diffAmountValue != 0;
            
            var isFactChanged = targetEntity.new_fact != null;

            if (isFactChanged)
            {
                var newAmount = new Money(targetEntity.new_amount.Value);

                if (!targetEntity.new_fact.GetValueOrDefault())
                {
                    newAmount.Value = currentAmount.Value * -1;
                }

                RecalculateAgreementPaidAmount(currentInvoice.new_dogovorid.Id, newAmount);
            }
            else
            {
                if (currentInvoice.new_fact.GetValueOrDefault() && isAmountChanged)
                {
                    RecalculateAgreementPaidAmount(currentInvoice.new_dogovorid.Id, new Money(diffAmountValue));
                }
            }
        }

        private void RecalculateAgreementPaidAmount(Guid id, Money factSumma)
        {
            var agreementService = new AgreementService(_service);
            var updatedAgreement = agreementService.RecalculateFactSumma(id, factSumma);
            _service.Update(updatedAgreement);
        }
    }
}