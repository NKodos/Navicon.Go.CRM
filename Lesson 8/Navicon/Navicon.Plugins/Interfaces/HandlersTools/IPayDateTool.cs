using Navicon.Common;
using Navicon.Common.Entities;

namespace Navicon.Plugins.Interfaces.HandlersTools
{
    public interface IPayDateTool
    {
        Result<new_invoice> SetDefaultPayDate(new_invoice targetInvoice);
    }
}