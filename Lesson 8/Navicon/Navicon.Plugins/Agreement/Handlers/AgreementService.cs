using System;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Navicon.Common.Entities;

namespace Navicon.Plugins.Agreement.Handlers
{
    public class AgreementService
    {
        private readonly IOrganizationService _service;

        public AgreementService(IOrganizationService service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        public void CheckContactFirstAgreementDate(new_agreement targetEntity)
        {
            
            var contactRef = targetEntity.new_contact;
            var contact = _service.Retrieve(contactRef.LogicalName, contactRef.Id,
                new ColumnSet(Contact.Fields.new_date)).ToEntity<Contact>();

            var query = new QueryExpression(new_agreement.EntityLogicalName)
            {
                ColumnSet = new ColumnSet(new_agreement.Fields.Id),
                NoLock = true,
                TopCount = 1
            };
            query.Criteria.AddCondition(new_agreement.Fields.new_contact, ConditionOperator.Equal, contactRef.Id);

            var result = _service.RetrieveMultiple(query);

            if (result.Entities.Count < 1)
            {
                var updatedContact = new Contact 
                {
                    Id = contact.Id,
                    new_date = targetEntity.new_date
                };

                _service.Update(updatedContact);
            }

        }

        public new_agreement RecalculateFactSumma(Guid agreementId, Money summa)
        {
            var agreement = _service.Retrieve(new_agreement.EntityLogicalName, agreementId,
                new ColumnSet(new_agreement.Fields.new_factsumma)).ToEntity<new_agreement>();
            if (agreement == null) return new new_agreement();

            var result = new new_agreement {Id = agreement.Id, new_factsumma = agreement.new_factsumma};
            result.new_factsumma = result.new_factsumma ?? new Money();
            result.new_factsumma.Value += summa.Value;
            return result;
        }
    }
}