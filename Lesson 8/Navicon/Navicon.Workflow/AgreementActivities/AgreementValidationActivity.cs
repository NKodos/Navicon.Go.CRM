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

        protected override void Execute(CodeActivityContext context)
        {
            var wfContext = context.GetExtension<IWorkflowContext>();
            var organizationServiceFactory = context.GetExtension<IOrganizationServiceFactory>();
            var service = organizationServiceFactory.CreateOrganizationService(null);

            var agreementRef = AgreementReference.Get(context);

            var query = new QueryExpression(new_invoice.EntityLogicalName)
            {
                ColumnSet = new ColumnSet(new_invoice.Fields.new_name),
                NoLock = true,
                TopCount = 1
            };
            query.Criteria.AddCondition(new_invoice.Fields.new_dogovorid, ConditionOperator.Equal, agreementRef.Id);
            var queryResult = service.RetrieveMultiple(query);

            IsValid.Set(context, queryResult.Entities.Count > 0);

        }
    }
}