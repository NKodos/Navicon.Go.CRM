using System;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Navicon.Common.Entities;
using Navicon.Common.Entities.Query;

namespace Navicon.Workflow.AgreementActivities.Handler
{
    public class AgreementService
    {
        private readonly IOrganizationService _service;

        public AgreementService(IOrganizationService service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        public void CreatePaymentSchedule(EntityReference agreementRef)
        {
            var columns = new ColumnSet(new_agreement.Fields.new_creditperiod,
                new_agreement.Fields.new_summa,
                new_agreement.Fields.new_creditperiod,
                new_agreement.Fields.new_number
            );

            var agreementEntity = _service.Retrieve(new_agreement.EntityLogicalName, agreementRef.Id, columns)
                .ToEntity<new_agreement>();

            if (agreementEntity.new_summa == null) throw new Exception("Сумма договора не указана");

            var monthCount = agreementEntity.new_creditperiod.GetValueOrDefault(0) * 12;
            if (monthCount == 0) throw new Exception("Срок кредита меньше года");

            var invoiceSumma = agreementEntity.new_summa.Value / monthCount;

            for (var i = 0; i < monthCount; i++)
            {
                _service.Create(new new_invoice
                {
                    new_name = $"Счет №{i+1} на оплату договора №{agreementEntity.new_number}",
                    new_dogovorid = agreementRef,
                    new_date = DateTime.Now,
                    new_type = new_invoice_new_type.__100000001,
                    new_fact = false,
                    new_amount = new Money(invoiceSumma)
                });
            }
        }

        public void DeleteAutomaticInvoice(EntityReference agreementRef)
        {
            var queryResult = new InvoiceQuery(_service)
                .AddCondition(
                    new ConditionExpression(new_invoice.Fields.new_dogovorid, ConditionOperator.Equal, agreementRef.Id),
                    new ConditionExpression(new_invoice.Fields.new_type, ConditionOperator.Equal, (int)new_invoice_new_type.__100000001))
                .GetAll();

            foreach (var entity in queryResult.Entities)
            {
                _service.Delete("new_invoice", entity.Id);
            }
        }
    }
}