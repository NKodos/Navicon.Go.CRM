using System;
using Microsoft.Xrm.Sdk;
using Navicon.Common.Entities;

namespace Navicon.Plugins.Invoice.Handlers
{
    public class InvoiceService
    {
        private readonly IOrganizationService _service;

        public InvoiceService(IOrganizationService service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        public void CheckInvoiceType(new_invoice targetInvoice)
        {
            if (targetInvoice.new_type == null)
            {
                targetInvoice.new_type = new_invoice_new_type.__100000000;
            }
        }
    }
}