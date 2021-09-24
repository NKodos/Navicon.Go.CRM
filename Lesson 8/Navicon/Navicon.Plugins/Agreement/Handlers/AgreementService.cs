using System;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Navicon.Common.Entities;
using Navicon.Common.Entities.Query;
using Navicon.Plugins.Interfaces;

namespace Navicon.Plugins.Agreement.Handlers
{
    /// <summary>
    /// Сервис для работы с договорами при Pre Operation
    /// </summary>
    public abstract class AgreementService : IService<new_agreement>
    {
        protected readonly IOrganizationService Service;

        public abstract void Execute(new_agreement entity);

        protected AgreementService(IOrganizationService service)
        {
            Service = service ?? throw new ArgumentNullException(nameof(service));
        }

        /// <summary>
        /// К текущей оплаченной сумме договора прибавить сумму, переданную в аргументе
        /// </summary>
        /// <param name="id">Guid договора</param>
        /// <param name="summa">Сумма, которая прибавится</param>
        /// <returns></returns>
        public new_agreement RecalculateFactSumma(Guid id, Money summa)
        {
            var agreement = new AgreementQuery(Service)
                .AddColumns(new_agreement.Fields.new_factsumma)
                .Get(id);

            var result = new new_agreement {Id = agreement.Id, new_factsumma = agreement.new_factsumma};
            result.new_factsumma = result.new_factsumma ?? new Money();
            result.new_factsumma.Value += summa.Value;
            return result;
        }

        /// <summary>
        /// Проверка: оплаченная сумма больше суммы договора
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool IsFactSummaGreaterAgreementSumma(Guid id)
        {
            var conlumns = new ColumnSet(new_agreement.Fields.new_factsumma, 
                new_agreement.Fields.new_summa);
            var agreement = Service.Retrieve(new_agreement.EntityLogicalName, id, conlumns)
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
        protected bool IsContactHasNotAgreement(Guid contactId)
        {
            var query = new AgreementQuery(Service);
            var condition = new ConditionExpression(new_agreement.Fields.new_contact, ConditionOperator.Equal, contactId);
            return !query.AddCondition(condition).HasData();
        }
    }
}