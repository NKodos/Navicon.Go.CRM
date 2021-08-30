using System;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Navicon.Common.Entities;
using Navicon.Common.Entities.Query;

namespace Navicon.Plugins.Agreement.Handlers
{
    /// <summary>
    /// Сервис для работы с договорами при Pre Operation
    /// </summary>
    public class AgreementService
    {
        private readonly IOrganizationService _service;

        public AgreementService(IOrganizationService service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        /// <summary>
        /// Обновить дату первого договора в объекте Котакт, указанным в договоре
        /// </summary>
        public void UpdateContactFirstAgreementDate(new_agreement targetEntity)
        {            
            var contactRef = targetEntity.new_contact;
            if (contactRef == null) return;

            var contact = _service.Retrieve(contactRef.LogicalName, contactRef.Id,
                new ColumnSet(Contact.Fields.new_date)).ToEntity<Contact>();

            if (IsContactHasNotAgreement(contactRef.Id))
            {
                var updatedContact = new Contact 
                {
                    Id = contact.Id,
                    new_date = targetEntity.new_date
                };

                _service.Update(updatedContact);
            }
        }

        public new_agreement RecalculateFactSumma(Guid id, Money summa)
        {
            var agreement = _service.Retrieve(new_agreement.EntityLogicalName, id,
                new ColumnSet(new_agreement.Fields.new_factsumma)).ToEntity<new_agreement>();
            if (agreement == null) return new new_agreement();

            var result = new new_agreement {Id = agreement.Id, new_factsumma = agreement.new_factsumma};
            result.new_factsumma = result.new_factsumma ?? new Money();
            result.new_factsumma.Value += summa.Value;
            return result;
        }

        public bool IsFactSummaGreaterAgreementSumma(Guid id)
        {
            var conlumns = new ColumnSet(new_agreement.Fields.new_factsumma, 
                new_agreement.Fields.new_summa);
            var agreement = _service.Retrieve(new_agreement.EntityLogicalName, id, conlumns)
                .ToEntity<new_agreement>();

            if (agreement.new_factsumma == null) return false;
            if (agreement.new_summa == null) return true;

            return agreement.new_factsumma.Value > agreement.new_summa.Value;
        }

        /// <summary>
        /// Проверка: контакт имеет хотя бы 1 договор?
        /// </summary>
        /// <param name="contactId">contact Guid</param>
        /// <returns>True - если у контакта есть какой-то договор</returns>
        private bool IsContactHasNotAgreement(Guid contactId)
        {
            var query = new AgreementQuery(_service);
            var condition = new ConditionExpression(new_agreement.Fields.new_contact, ConditionOperator.Equal, contactId);
            return !query.HasData(condition);
        }
    }
}