using System;
using Microsoft.Xrm.Sdk;
using Navicon.Common.Entities;

namespace Navicon.Plugins.Agreement.Handlers
{
    public class PostAgreementCreateService : AgreementService
    {
        public PostAgreementCreateService(IOrganizationService service) : base(service)
        {
        }

        public override void Execute(new_agreement entity)
        {
            TrySetFact(entity);
        }

        protected void TrySetFact(new_agreement targetAgreement)
        {
            var factSumma = targetAgreement.new_factsumma ?? new Money(0);
            var summa = targetAgreement.new_summa ?? new Money(0);

            if (factSumma.Value == summa.Value)
            {
                var updatedAgreement = new new_agreement { Id = targetAgreement.Id, new_fact = true };
                Service.Update(updatedAgreement);
            }
        }

    }
}