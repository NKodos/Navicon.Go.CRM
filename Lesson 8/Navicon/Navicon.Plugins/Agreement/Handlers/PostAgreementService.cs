using System;
using Microsoft.Xrm.Sdk;
using Navicon.Common.Entities;

namespace Navicon.Plugins.Agreement.Handlers
{
    public class PostAgreementService
    {
        private readonly IOrganizationService _service;

        public PostAgreementService(IOrganizationService service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        public void TrySetFact(new_agreement targetAgreement)
        {
            var factSumma = targetAgreement.new_factsumma ?? new Money(0);
            var summa = targetAgreement.new_summa ?? new Money(0);

            if (factSumma.Value == summa.Value)
            {
                var updatedAgreement = new new_agreement {Id = targetAgreement.Id, new_fact = true};
                _service.Update(updatedAgreement);
            }
        }
    }
}