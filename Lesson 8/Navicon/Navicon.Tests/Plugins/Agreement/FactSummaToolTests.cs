using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Moq;
using Navicon.Common.Entities;
using Navicon.Plugins.Agreement.Handlers.Tools;

namespace Navicon.Tests.Plugins.Agreement
{
    [TestClass]
    public class FactSummaToolTests
    {
        private readonly Mock<IOrganizationService> _serviceMock = new Mock<IOrganizationService>();

        [TestMethod]
        public void AddToFactSumma_FactSumma100Plus100_ReturnNewFactSumma200()
        {
            // Arrage
            var tool = new FactSummaTool(_serviceMock.Object);
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
            var agreementResult = tool.AddToFactSumma(currentAgreement, new Money(100));

            // Assert
            Assert.IsTrue(agreementResult.Success);
            Assert.AreEqual(agreementResult.Value.new_factsumma.Value, 200);
        }

        [TestMethod]
        public void IsFactSummaGreaterAgreementSumma_CompareFactSummaWithSumma_ReturnTrue()
        {
            // Arrage
            var agreementService = new FactSummaTool(_serviceMock.Object);
            var agreementId = Guid.NewGuid();

            _serviceMock
                .SetupSequence(service => service.Retrieve(new_agreement.EntityLogicalName,
                    agreementId, It.IsAny<ColumnSet>()))
                .Returns(new new_agreement
                {
                    Id = agreementId,
                    new_factsumma = new Money(100),
                    new_summa = new Money(1)
                })
                .Returns(new new_agreement
                {
                    Id = agreementId,
                    new_factsumma = new Money(100),
                    new_summa = new Money(100)
                });

            // Act
            var resultWhenGreater = agreementService.IsFactSummaGreaterAgreementSumma(agreementId);
            var resultWhenEqual = agreementService.IsFactSummaGreaterAgreementSumma(agreementId);

            // Assert
            Assert.IsTrue(resultWhenGreater);
            Assert.IsFalse(resultWhenEqual);
        }
    }
}
