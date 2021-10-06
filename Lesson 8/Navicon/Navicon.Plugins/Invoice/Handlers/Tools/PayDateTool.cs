using System;
using Navicon.Common;
using Navicon.Common.Entities;
using Navicon.Plugins.Interfaces.HandlersTools;

namespace Navicon.Plugins.Invoice.Handlers.Tools
{
    public class PayDateTool : IPayDateTool
    {
        public Result<new_invoice> SetDefaultPayDate(new_invoice targetInvoice)
        {
            try
            {
                var resultValue = new new_invoice
                {
                    Id = targetInvoice.Id
                };

                if (targetInvoice.new_fact.GetValueOrDefault())
                {
                    resultValue.new_paydate = DateTime.Now;
                    return Result.Ok(resultValue);
                }

                return Result.Fail<new_invoice>("Счет не оплачен");
            }
            catch (Exception ex)
            {
                return Result.Fail<new_invoice>(ex.Message);
            }
        }
    }
}