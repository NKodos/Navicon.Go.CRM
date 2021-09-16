using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Moq;
using Navicon.Common.Entities;
using Navicon.Plugins.Invoice.Handlers;

namespace Navicon.Tests.Plugins.Invoice
{
    [TestClass]
    public class InvoiceServiceTests
    {
        private readonly Mock<IOrganizationService> _serviceMock = new Mock<IOrganizationService>();

        [TestMethod]
        public void CheckAgreementPaidAmount_FactSummaIsNotGreaterAgreementSumma_NoException()
        {
            // Arrange
            var invoiceService = new ToolsInvoiceService(_serviceMock.Object); 

            _serviceMock
                .Setup(service => service.Retrieve(new_agreement.EntityLogicalName,
                    It.IsAny<Guid>(), It.IsAny<ColumnSet>()))
                .Returns(new new_agreement
                {
                    new_factsumma = null
                });

            var targetInvoice = new new_invoice
            {
                new_dogovorid = new EntityReference {Id = Guid.NewGuid() },
                new_fact = false
            };
            
            // Act
            invoiceService.CheckAgreementPaidAmount(targetInvoice);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void CheckAgreementPaidAmount_FactSummaIsGreaterAgreementSumma_ThrowException()
        {
            // Arrange
            var invoiceService = new ToolsInvoiceService(_serviceMock.Object);

            _serviceMock
                .Setup(service => service.Retrieve(new_agreement.EntityLogicalName,
                    It.IsAny<Guid>(), It.IsAny<ColumnSet>()))
                .Returns(new new_agreement
                {
                    new_factsumma = new Money(2),
                    new_summa = new Money(1)
                });

            var targetInvoice = new new_invoice
            {
                new_dogovorid = new EntityReference { Id = Guid.NewGuid() },
                new_fact = false
            };

            // Act
            invoiceService.CheckAgreementPaidAmount(targetInvoice);
        }
    }
}
