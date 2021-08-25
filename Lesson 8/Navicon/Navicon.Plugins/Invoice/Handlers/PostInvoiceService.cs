using System;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Navicon.Common.Entities;
using Navicon.Plugins.Agreement.Handlers;

namespace Navicon.Plugins.Invoice.Handlers
{
    public class PostInvoiceService
    {
        private readonly IOrganizationService _service;

        public PostInvoiceService(IOrganizationService service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        public void SetPayDate(new_invoice targetInvoice)
        {
            if (targetInvoice.new_fact.GetValueOrDefault())
            {
                targetInvoice.new_paydate = DateTime.Now;
                var updatedInvoice = new new_invoice
                {
                    Id = targetInvoice.Id,
                    new_paydate = DateTime.Now
                };

                _service.Update(updatedInvoice);
            }

        }
    }
}