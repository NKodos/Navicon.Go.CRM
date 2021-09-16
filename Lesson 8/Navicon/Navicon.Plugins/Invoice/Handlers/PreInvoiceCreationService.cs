using Microsoft.Xrm.Sdk;
using Navicon.Common.Entities;

namespace Navicon.Plugins.Invoice.Handlers
{
    public class PreInvoiceCreationService : InvoiceService
    {
        public PreInvoiceCreationService(IOrganizationService service) : base(service)
        {
        }

        public override void Execute(new_invoice targetInvoice)
        {
            SetDefaultInvoiceType(targetInvoice);
            AddAgreementPaidAmount(targetInvoice);
            CheckAgreementPaidAmount(targetInvoice);
        }


        /// <summary>
        /// Установить тип счета по-умолчанию (Ручное создание)
        /// </summary>
        protected void SetDefaultInvoiceType(new_invoice targetInvoice)
        {
            if (targetInvoice.new_type == null)
            {
                targetInvoice.new_type = new_invoice_new_type.__100000000;
            }
        }

        /// <summary>
        /// Добавить сумму счета к оплаченной сумме договора
        /// </summary>
        protected void AddAgreementPaidAmount(new_invoice targetInvoice)
        {
            if (!targetInvoice.new_fact.GetValueOrDefault()) return;

            RecalculateAgreementPaidAmount(targetInvoice.new_dogovorid.Id, targetInvoice.new_amount);
        }
    }
}