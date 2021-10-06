using System;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Navicon.Common;
using Navicon.Common.Entities;

namespace Navicon.Plugins.Agreement.Handlers.Tools
{
    public interface IFactSummaTool
    {
        Result<new_agreement> AddToFactSumma(new_agreement agreement, Money summa);

        bool IsFactSummaGreaterAgreementSumma(Guid id);
    }

    public class FactSummaTool : IFactSummaTool
    {
        private readonly IOrganizationService _service;

        public FactSummaTool(IOrganizationService service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        /// <summary>
        /// К оплаченной сумме договора прибавить сумму, переданную в аргументе
        /// </summary>
        /// <param name="agreement">Объект, к которму нужно прибавить</param>
        /// <param name="summa">Сумма, которая прибавится</param>
        /// <returns></returns>
        public Result<new_agreement> AddToFactSumma(new_agreement agreement, Money summa)
        {
            try
            {
                var resultValue = new new_agreement { Id = agreement.Id, new_factsumma = agreement.new_factsumma };
                resultValue.new_factsumma = resultValue.new_factsumma ?? new Money();
                resultValue.new_factsumma.Value += summa.Value;
                return Result.Ok(resultValue);
            }
            catch (Exception ex)
            {
                return Result.Fail<new_agreement>(ex.Message);
            }
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
            var agreement = _service.Retrieve(new_agreement.EntityLogicalName, id, conlumns)
                .ToEntity<new_agreement>();

            if (agreement.new_factsumma == null) return false;
            if (agreement.new_summa == null) return true;

            return agreement.new_factsumma.Value > agreement.new_summa.Value;
        }
    }
}