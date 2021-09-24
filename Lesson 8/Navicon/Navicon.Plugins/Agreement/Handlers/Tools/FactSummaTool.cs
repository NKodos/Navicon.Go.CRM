using System;
using Microsoft.Xrm.Sdk;
using Navicon.Common;
using Navicon.Common.Entities;

namespace Navicon.Plugins.Agreement.Handlers.Tools
{
    public interface IFactSummaTool
    {
        Result<new_agreement> AddToFactSumma(new_agreement agreement, Money summa);
    }

    public class FactSummaTool : IFactSummaTool
    {
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
    }
}