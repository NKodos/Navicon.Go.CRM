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

        /// <summary>
        /// Установить тип счета по-умолчанию (Ручное создание)
        /// </summary>
        public void SetDefaultInvoiceType(new_invoice targetInvoice)
        {
            if (targetInvoice.new_type == null)
            {
                targetInvoice.new_type = new_invoice_new_type.__100000000;
            }
        }

        /// <summary>
        /// Добавить сумму счета к оплаченной сумме договора
        /// </summary>
        public void AddAgreementPaidAmount(new_invoice targetInvoice)
        {
            if (!targetInvoice.new_fact.GetValueOrDefault()) return;

            RecalculateAgreementPaidAmount(targetInvoice.new_dogovorid.Id, targetInvoice.new_amount);
        }

        /// <summary>
        /// Вычесть сумму счета из оплаченной суммы договора
        /// </summary>
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

        /// <summary>
        /// Пересчитать и обновить сумму договора
        /// </summary>
        /// <param name="id">Guid договора</param>
        /// <param name="factSumma">Сумма, которую нужно прибавить</param>
        private void RecalculateAgreementPaidAmount(Guid id, Money factSumma)
        {
            var agreementService = new AgreementService(_service);
            var updatedAgreement = agreementService.RecalculateFactSumma(id, factSumma);
            _service.Update(updatedAgreement);
        }

        /// <summary>
        /// Проверка: если у договора оплаченная сумма больше суммы договора, тогда прокинуть исключение
        /// </summary>
        public void CheckAgreementPaidAmount(new_invoice targetInvoice)
        {
            var dogovorIdAndIsFact = GetDogovorIdAndIsFact(targetInvoice);

            var agreementService = new AgreementService(_service);
            if (agreementService.IsFactSummaGreaterAgreementSumma(dogovorIdAndIsFact.id))
            {
                throw new Exception("Оплаченная сумма выбранного договора превышает сумму договора");
            }
        }

        /// <summary>
        /// Если id договора или факт не были изменены - получить их из БД
        /// </summary>
        /// <returns>Кортеж: Guid договора и поле Оплачен</returns>
        private (Guid id, bool isFact) GetDogovorIdAndIsFact(new_invoice targetInvoice)
        {
            var result = (id: Guid.Empty, isFact: false);

            if (targetInvoice.new_dogovorid == null || targetInvoice.new_fact == null)
            {
                var currentInvoice = _service.Retrieve(new_invoice.EntityLogicalName, targetInvoice.Id,
                        new ColumnSet(new_invoice.Fields.new_dogovorid, new_invoice.Fields.new_fact))
                    .ToEntity<new_invoice>();
                result.id = currentInvoice.new_dogovorid.Id;
                result.isFact = currentInvoice.new_fact.GetValueOrDefault();
            }

            return result;
        }
    }
}