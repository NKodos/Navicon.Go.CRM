using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Moq;
using Navicon.Common.Entities;
using Navicon.Plugins.Agreement.Handlers;

namespace Navicon.Tests.Plugins.Agreement
{
    [TestClass]
    public class AgreementServiceTests
    {
        private readonly Mock<IOrganizationService> _serviceMock = new Mock<IOrganizationService>();

        [TestMethod]
        public void RecalculateFactSumma_FactSumma100Plus100_ReturnNewFactSumma200()
        {
            // Arrage
            var agreementService = new AgreementService(_serviceMock.Object);
            var agreementId = Guid.NewGuid();
            var currentAgreement = new new_agreement
            {
                Id = agreementId,
                new_factsumma = new Money(100)
            };

            _serviceMock
                .Setup(service => service.Retrieve(new_agreement.EntityLogicalName,
                    agreementId, It.IsAny<ColumnSet>()))
                .Returns(currentAgreement);

            // Act
            var agreementResult = agreementService.RecalculateFactSumma(agreementId, new Money(100));

            // Assert
            Assert.AreEqual(agreementResult.new_factsumma.Value, 200);
        }
    }
}
