using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Moq;
using Navicon.Common.Entities;
using Navicon.Common.Entities.Query;
using Navicon.Plugins.Communication.Handler;

namespace Navicon.Tests.Plugins.Agreement
{
    [TestClass]
    public class CommunicationServiceTests
    {
        private readonly Mock<IOrganizationService> _serviceMock = new Mock<IOrganizationService>();

        [TestMethod]
        public void CheckNewCommunicationMain_MainCommunicationIsExists_NoException()
        {
            // Arrange
            _serviceMock
                .Setup(service => service.RetrieveMultiple(It.IsAny<QueryBase>()))
                .Returns(new EntityCollection(new List<Entity>()));

            var communicationService = new CommunicationService(_serviceMock.Object);
            var targetEntity = new new_communication
            {
                new_main = true,
                new_type = new_communication_new_type.Email,
                new_contactid = new EntityReference { Id = Guid.NewGuid()}
            };

            // Act
            communicationService.CheckNewCommunicationMain(targetEntity);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void CheckNewCommunicationMain_MainCommunicationIsExists_ThrowException()
        {
            // Arrange
            _serviceMock
                .Setup(service => service.RetrieveMultiple(It.IsAny<QueryBase>()))
                .Returns(new EntityCollection(new List<Entity>
                {
                    new Entity()
                }));

            var communicationService = new CommunicationService(_serviceMock.Object);
            var targetEntity = new new_communication
            {
                new_main = true,
                new_type = new_communication_new_type.Email,
                new_contactid = new EntityReference { Id = Guid.NewGuid() }
            };

            // Act
            communicationService.CheckNewCommunicationMain(targetEntity);
        }
    }
}
