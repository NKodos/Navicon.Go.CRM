using System.Activities;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Workflow;
using Navicon.Common.Entities;

namespace Navicon.Workflow.AgreementActivities
{
    public class AgreementValidationActivity : CodeActivity
    {
        [Input("Agreement")]
        [RequiredArgument]
        [ReferenceTarget("new_agreement")]
        public InArgument<EntityReference> AgreementReference { get; set; }

        [Output("Is Agreement valid")]
        public OutArgument<bool> IsValid { get; set; }

        [Output("The agreement has a paid invoice")]
        public OutArgument<bool> HasPaidInvoice { get; set; }

        [Output("The agreement has a manual type invoice")]
        public OutArgument<bool> HasManualTypeInvoice { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            var organizationServiceFactory = context.GetExtension<IOrganizationServiceFactory>();
            var service = organizationServiceFactory.CreateOrganizationService(null);

            ValidateHavingLinkedInvoice(context, service);
            ValidateHavingPaidInvoice(context, service);
            ValidateHavingManualTypeInvoice(context, service);
        }

        private void ValidateHavingLinkedInvoice(CodeActivityContext context, IOrganizationService service)
        {
            var query = CreateGettingInvoiceQuery(context);

            var result = service.RetrieveMultiple(query);
            IsValid.Set(context, result.Entities.Count > 0);
        }

        private void ValidateHavingPaidInvoice(CodeActivityContext context, IOrganizationService service)
        {
            var query = CreateGettingInvoiceQuery(context);
            query.Criteria.AddCondition(new_invoice.Fields.new_fact, ConditionOperator.Equal, true);

            var result = service.RetrieveMultiple(query);
            HasPaidInvoice.Set(context, result.Entities.Count > 0);
        }

        private void ValidateHavingManualTypeInvoice(CodeActivityContext context, IOrganizationService service)
        {
            var query = CreateGettingInvoiceQuery(context);
            query.Criteria.AddCondition(new_invoice.Fields.new_type, ConditionOperator.Equal, (int)new_invoice_new_type.__100000000);

            var result = service.RetrieveMultiple(query);
            HasManualTypeInvoice.Set(context, result.Entities.Count > 0);
        }

        private QueryExpression CreateGettingInvoiceQuery(CodeActivityContext context)
        {
            var agreementRef = AgreementReference.Get(context);
            var query = new QueryExpression(new_invoice.EntityLogicalName)
            {
                ColumnSet = new ColumnSet(new_invoice.Fields.new_name),
                NoLock = true,
                TopCount = 1
            };
            query.Criteria.AddCondition(new_invoice.Fields.new_dogovorid, ConditionOperator.Equal, agreementRef.Id);
            return query;
        }
    }
}