using Navicon.Common;
using Navicon.Common.Entities;

namespace Navicon.Plugins.Interfaces.HandlersTools
{
    public interface IFactTool
    {
        Result<new_agreement> TrySetFact(new_agreement targetAgreement);
    }
}