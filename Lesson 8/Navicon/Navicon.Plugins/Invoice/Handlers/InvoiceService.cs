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

        public void AddAgreementPaidAmount(new_invoice targetInvoice)
        {
            if (!targetInvoice.new_fact.GetValueOrDefault()) return;

            RecalculateAgreementPaidAmount(targetInvoice.new_dogovorid.Id, targetInvoice.new_amount);
        }

        public void SubAgreementPaidAmount(new_invoice targetInvoice)
        {
            if (!targetInvoice.new_fact.GetValueOrDefault()) return;

            var newAmount = new Money(targetInvoice.new_amount.Value * -1);
            RecalculateAgreementPaidAmount(targetInvoice.new_dogovorid.Id, newAmount);
        }

        public void UpdateAgreementPaidAmount(new_invoice targetInvoice)
        {
            var columnSet = new ColumnSet(new_invoice.Fields.new_amount, 
                new_invoice.Fields.new_fact, new_invoice.Fields.new_dogovorid);
            var currentInvoice = _service.Retrieve(new_invoice.EntityLogicalName, targetInvoice.Id, columnSet)
                .ToEntity<new_invoice>();

            currentInvoice.new_amount = currentInvoice.new_amount ?? new Money(0);
            targetInvoice.new_amount = targetInvoice.new_amount ?? new Money(currentInvoice.new_amount.Value);

            var currentAmount = currentInvoice.new_amount;
            var diffAmountValue = targetInvoice.new_amount.Value - currentAmount.Value;
            var isAmountChanged = diffAmountValue != 0;
            
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

        public void CheckAgreementPaidAmount(new_invoice targetInvoice)
        {
            var dogovorId = GetDogovorId(targetInvoice);
            if (dogovorId == null) return;

            var fact = GetFact(targetInvoice);
            if (!fact.GetValueOrDefault()) return;

            var agreementService = new AgreementService(_service);
            if (agreementService.IsFactSummaGreaterAgreementSumma(dogovorId.Value))
            {
                throw new Exception("Оплаченная сумма выбранного договора превышает сумму договора");
            }
        }

        private Guid? GetDogovorId(new_invoice targetInvoice)
        {
            var dogovorId = targetInvoice.new_dogovorid?.Id;
            if (dogovorId == null)
            {
                var currentInvoice = _service.Retrieve(new_invoice.EntityLogicalName, targetInvoice.Id, 
                        new ColumnSet(new_invoice.Fields.new_dogovorid))
                        .ToEntity<new_invoice>();

                dogovorId = currentInvoice.new_dogovorid?.Id;
            }

            return dogovorId;
        }

        private bool? GetFact(new_invoice targetInvoice)
        {
            var dogovorId = targetInvoice.new_fact;
            if (dogovorId == null)
            {
                var currentInvoice = _service.Retrieve(new_invoice.EntityLogicalName, targetInvoice.Id,
                        new ColumnSet(new_invoice.Fields.new_fact))
                    .ToEntity<new_invoice>();

                dogovorId = currentInvoice.new_fact;
            }

            return dogovorId;
        }
    }
}