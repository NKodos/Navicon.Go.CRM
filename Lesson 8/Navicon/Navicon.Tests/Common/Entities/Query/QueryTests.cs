using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Moq;
using Navicon.Common.Entities;
using Navicon.Common.Entities.Query;

namespace Navicon.Tests.Common.Entities.Query
{
    [TestClass]
    public class QueryTests
    {
        [TestMethod]
        public void EntityQuery_HasData_ReturnTrue()
        {
            // Arrange

            var entityCollection = new EntityCollection(new List<Entity>
            {
                new Entity()
            });

            var serviceMock = new Mock<IOrganizationService>();
            serviceMock
                .Setup(service => service.RetrieveMultiple(It.IsAny<QueryExpression>()))
                .Returns(entityCollection);

            var query = new EntityQueryTested(serviceMock.Object);

            // Act
            var hasData = query.HasData();

            // Assert
            Assert.IsTrue(hasData);
        }
    }

    public class EntityQueryTested : EntityQuery<new_invoice>
    {
        public EntityQueryTested(IOrganizationService service) : base(service, "TestEntity")
        {
        }
    }
}
