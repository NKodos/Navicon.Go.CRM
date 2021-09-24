using Microsoft.Xrm.Sdk;
using Navicon.Common;
using Navicon.Common.Entities;

namespace Navicon.Plugins.Agreement.Handlers.Tools
{
    public interface IAgreementFactTool
    {
        Result<new_agreement> TrySetFact(new_agreement targetAgreement);
    }

    public class AgreementFactTool : IAgreementFactTool
    {
        public Result<new_agreement> TrySetFact(new_agreement targetAgreement)
        {
            var factSumma = targetAgreement.new_factsumma ?? new Money(0);
            var summa = targetAgreement.new_summa ?? new Money(0);

            return factSumma.Value == summa.Value ? 
                Result.Ok(new new_agreement { Id = targetAgreement.Id, new_fact = true }) : 
                Result.Fail<new_agreement>("Сумма договора не равняется оплаченной сумме");
        }
    }
}