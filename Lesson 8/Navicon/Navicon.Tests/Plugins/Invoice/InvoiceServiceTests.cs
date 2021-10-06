using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Moq;
using Navicon.Common.Entities;
using Navicon.Plugins.Agreement.Handlers.Tools;

namespace Navicon.Tests.Plugins.Invoice
{
    // TODO: тестировать не сервисы, а tools
    [TestClass]
    public class InvoiceServiceTests
    {
        private readonly Mock<IOrganizationService> _serviceMock = new Mock<IOrganizationService>();

        // TODO: тестировать не сервисы, а tools
        [TestMethod]
        public void CheckAgreementPaidAmount_FactSummaIsNotGreaterAgreementSumma_ReturnFalse()
        {
            // Arrange
            var invoiceService = new FactSummaTool(_serviceMock.Object);

            _serviceMock
                .Setup(service => service.Retrieve(new_agreement.EntityLogicalName,
                    It.IsAny<Guid>(), It.IsAny<ColumnSet>()))
                .Returns(new new_agreement
                {
                    new_factsumma = null
                });

            var targetInvoice = new new_invoice
            {
                new_dogovorid = new EntityReference { Id = Guid.NewGuid() },
                new_fact = false
            };

            // Act
            var result = invoiceService.IsFactSummaGreaterAgreementSumma(targetInvoice.Id);

            // Assert
            Assert.IsFalse(result);
        }

        // TODO: тестировать не сервисы, а tools
        [TestMethod]
        public void CheckAgreementPaidAmount_FactSummaIsGreaterAgreementSumma_ReturnTrue()
        {
            // Arrange
            var invoiceService = new FactSummaTool(_serviceMock.Object);

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
            var result = invoiceService.IsFactSummaGreaterAgreementSumma(targetInvoice.Id);
            
            // Assert
            Assert.IsTrue(result);
        }
    }
}
