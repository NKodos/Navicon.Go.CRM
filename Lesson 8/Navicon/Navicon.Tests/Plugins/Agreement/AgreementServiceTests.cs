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
        // TODO: исправить 
        //[TestMethod]
        //public void RecalculateFactSumma_FactSumma100Plus100_ReturnNewFactSumma200()
        //{
        //    // Arrange
        //    var agreementService = new ToolsAgreementService(_serviceMock.Object);
        //    var agreementId = Guid.NewGuid();
        //    var currentAgreement = new new_agreement
        //    {
        //        Id = agreementId,
        //        new_factsumma = new Money(100)
        //    };

        //    _serviceMock
        //        .Setup(service => service.Retrieve(new_agreement.EntityLogicalName,
        //            agreementId, It.IsAny<ColumnSet>()))
        //        .Returns(currentAgreement);

        //    // Act
        //    var agreementResult = agreementService.RecalculateFactSumma(agreementId, new Money(100));

        //    // Assert
        //    Assert.AreEqual(agreementResult.new_factsumma.Value, 200);
        //}

        //[TestMethod]
        //public void RecalculateFactSumma_FactSummaNullPlus100_ReturnNewFactSumma100()
        //{
        //    // Arrage
        //    var agreementService = new ToolsAgreementService(_serviceMock.Object);
        //    var agreementId = Guid.NewGuid();
        //    var currentAgreement = new new_agreement
        //    {
        //        Id = agreementId,
        //        new_factsumma = null
        //    };

        //    _serviceMock
        //        .Setup(service => service.Retrieve(new_agreement.EntityLogicalName,
        //            agreementId, It.IsAny<ColumnSet>()))
        //        .Returns(currentAgreement);

        //    // Act
        //    var agreementResult = agreementService.RecalculateFactSumma(agreementId, new Money(100));

        //    // Assert
        //    Assert.AreEqual(agreementResult.new_factsumma.Value, 100);
        //}

        //[TestMethod]
        //public void IsFactSummaGreaterAgreementSumma_CompareFactSummaWithSumma_ReturnTrue()
        //{
        //    // Arrage
        //    var agreementService = new ToolsAgreementService(_serviceMock.Object);
        //    var agreementId = Guid.NewGuid();
        //    var currentAgreement = new new_agreement
        //    {
        //        Id = agreementId,
        //        new_factsumma = new Money(100)
        //    };

        //    _serviceMock
        //        .SetupSequence(service => service.Retrieve(new_agreement.EntityLogicalName,
        //            agreementId, It.IsAny<ColumnSet>()))
        //        .Returns(new new_agreement
        //        {
        //            Id = agreementId,
        //            new_factsumma = new Money(100),
        //            new_summa = new Money(1)
        //        })
        //        .Returns(new new_agreement
        //        {
        //            Id = agreementId,
        //            new_factsumma = new Money(100),
        //            new_summa = new Money(100)
        //        });

        //    // Act
        //    var resultWhenGreater = agreementService.IsFactSummaGreaterAgreementSumma(agreementId);
        //    var resultWhenEqual = agreementService.IsFactSummaGreaterAgreementSumma(agreementId);

        //    // Assert
        //    Assert.IsTrue(resultWhenGreater);
        //    Assert.IsFalse(resultWhenEqual);
        //}
    }
}
