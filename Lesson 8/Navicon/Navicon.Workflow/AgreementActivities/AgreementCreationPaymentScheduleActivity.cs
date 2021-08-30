using System;
using System.Activities;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Workflow;
using Navicon.Common.Entities;
using Navicon.Workflow.AgreementActivities.Handler;

namespace Navicon.Workflow.AgreementActivities
{
    public class AgreementCreationPaymentScheduleActivity : CodeActivity
    {
        [Input("Agreement")]
        [RequiredArgument]
        [ReferenceTarget("new_agreement")]
        public InArgument<EntityReference> AgreementReference { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            var organizationServiceFactory = context.GetExtension<IOrganizationServiceFactory>();
            var service = organizationServiceFactory.CreateOrganizationService(null);
            var agreementRef = AgreementReference.Get(context);
            try
            {
                var agreementService = new AgreementService(service);
                agreementService.CreatePaymentSchedule(agreementRef);
            }
            catch (Exception ex)
            {
                throw new InvalidWorkflowException(ex.Message, ex);
            }
        }
    }
}