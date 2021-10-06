using System;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Navicon.Common.Entities;
using Navicon.Common.Entities.Query;

namespace Navicon.Plugins.Agreement.Handlers
{
    public class PreAgreementCreateService : AgreementService
    {
        public PreAgreementCreateService(IOrganizationService service) : base(service)
        {
        }

        public override void Execute(new_agreement entity)
        {
            UpdateContactFirstAgreementDate(entity);
        }

        /// <summary>
        /// Обновить дату первого договора в объекте Котакт, указанным в договоре
        /// </summary>
        protected void UpdateContactFirstAgreementDate(new_agreement targetEntity)
        {
            var contactRef = targetEntity.new_contact;
            if (contactRef == null) return;

            var contact = Service.Retrieve(contactRef.LogicalName, contactRef.Id,
                new ColumnSet(Contact.Fields.new_date)).ToEntity<Contact>();

            if (contact == null) throw new Exception("Контакт договора не найден в БД. Id контакта = " + contactRef.Id);

            if (IsContactHasNotAgreement(contactRef.Id))
            {
                var updatedContact = new Contact
                {
                    Id = contact.Id,
                    new_date = targetEntity.new_date
                };

                Service.Update(updatedContact);
            }
        }
        
        /// <summary>
        /// Проверка: контакт имеет хотя бы 1 договор?
        /// </summary>
        /// <param name="contactId">contact Guid</param>
        /// <returns>True - если у контакта есть какой-то договор</returns>
        protected bool IsContactHasNotAgreement(Guid contactId)
        {
            var query = new AgreementQuery(Service);
            var condition = new ConditionExpression(new_agreement.Fields.new_contact, ConditionOperator.Equal, contactId);
            return !query.AddCondition(condition).HasData();
        }
    }
}