using System;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Navicon.Common.Entities;
using Navicon.Plugins.Agreement.Handlers;
using Navicon.Plugins.Interfaces;

namespace Navicon.Plugins.Invoice.Handlers
{
    public abstract class InvoiceService : IService<new_invoice>
    {
        protected readonly IOrganizationService Service;

        protected InvoiceService(IOrganizationService service)
        {
            Service = service ?? throw new ArgumentNullException(nameof(service));
        }

        public abstract void Execute(new_invoice invoice);


        /// <summary>
        /// Вычесть сумму счета из оплаченной суммы договора
        /// </summary>
        protected void SubAgreementPaidAmount(new_invoice targetInvoice)
        {
            if (!targetInvoice.new_fact.GetValueOrDefault()) return;

            var newAmount = new Money(targetInvoice.new_amount.Value * -1);
            RecalculateAgreementPaidAmount(targetInvoice.new_dogovorid.Id, newAmount);
        }

        /// <summary>
        /// Пересчитать и обновить сумму договора
        /// </summary>
        /// <param name="id">Guid договора</param>
        /// <param name="factSumma">Сумма, которую нужно прибавить</param>
        protected void RecalculateAgreementPaidAmount(Guid id, Money factSumma)
        {
            var agreementService = new ToolsAgreementService(Service);
            var updatedAgreement = agreementService.RecalculateFactSumma(id, factSumma);
            Service.Update(updatedAgreement);
        }

        /// <summary>
        /// Проверка: если у договора оплаченная сумма больше суммы договора, тогда прокинуть исключение
        /// </summary>
        public void CheckAgreementPaidAmount(new_invoice targetInvoice)
        {
            var dogovorIdAndIsFact = GetDogovorIdAndIsFact(targetInvoice);

            var agreementService = new ToolsAgreementService(Service);
            if (agreementService.IsFactSummaGreaterAgreementSumma(dogovorIdAndIsFact.id))
            {
                throw new Exception("Оплаченная сумма выбранного договора превышает сумму договора");
            }
        }

        /// <summary>
        /// Если id договора или факт не были изменены - получить их из БД
        /// </summary>
        /// <returns>Кортеж: Guid договора и поле Оплачен</returns>
        protected (Guid id, bool isFact) GetDogovorIdAndIsFact(new_invoice targetInvoice)
        {
            var result = (id: Guid.Empty, isFact: false);

            if (targetInvoice.new_dogovorid == null || targetInvoice.new_fact == null)
            {
                var currentInvoice = Service.Retrieve(new_invoice.EntityLogicalName, targetInvoice.Id,
                        new ColumnSet(new_invoice.Fields.new_dogovorid, new_invoice.Fields.new_fact))
                    .ToEntity<new_invoice>();
                result.id = currentInvoice.new_dogovorid.Id;
                result.isFact = currentInvoice.new_fact.GetValueOrDefault();
            }

            return result;
        }

        protected void SetPayDate(new_invoice targetInvoice)
        {
            if (targetInvoice.new_fact.GetValueOrDefault())
            {
                targetInvoice.new_paydate = DateTime.Now;
                var updatedInvoice = new new_invoice
                {
                    Id = targetInvoice.Id,
                    new_paydate = DateTime.Now
                };

                Service.Update(updatedInvoice);
            }
        }

    }
}