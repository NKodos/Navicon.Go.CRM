using Microsoft.Xrm.Sdk;
using Navicon.Common.Entities;
using Navicon.Plugins.Agreement.Handlers.Tools;

namespace Navicon.Plugins.Invoice.Handlers
{
    public class PreInvoiceCreationService : InvoiceService
    {
        private readonly IFactSummaTool _factSummaTool;

        public PreInvoiceCreationService(IOrganizationService service, 
            IFactSummaTool factSummaTool) : base(service)
        {
            _factSummaTool = factSummaTool;
        }

        public override void Execute(new_invoice targetInvoice)
        {
            SetDefaultInvoiceType(targetInvoice);
            AddAgreementPaidAmount(targetInvoice);
            CheckAgreementPaidAmount(targetInvoice, _factSummaTool);
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

            RecalculateAgreementPaidAmount(_factSummaTool, targetInvoice.new_dogovorid.Id, targetInvoice.new_amount);
        }
    }
}