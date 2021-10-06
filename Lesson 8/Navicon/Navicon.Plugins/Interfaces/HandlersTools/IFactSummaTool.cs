using System;
using Microsoft.Xrm.Sdk;
using Navicon.Common;
using Navicon.Common.Entities;

namespace Navicon.Plugins.Interfaces.HandlersTools
{
    public interface IFactSummaTool
    {
        Result<new_agreement> AddToFactSumma(new_agreement agreement, Money summa);

        bool IsFactSummaGreaterAgreementSumma(Guid id);
    }
}