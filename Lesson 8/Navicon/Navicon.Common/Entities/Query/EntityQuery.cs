using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace Navicon.Common.Entities.Query
{
    public class EntityQuery
    {
        public readonly string EntityName;

        public IOrganizationService Service { get; protected set; }

        public EntityQuery(IOrganizationService service, string entityName)
        {
            Service = service;
            EntityName = entityName;
        }

        public bool HasData(params ConditionExpression[] conditionExpression)
        {
            var query = new EntityQueryExpression(Service, EntityName, 1);
            query.AddCondition(conditionExpression);

            var entityCollection = query.RetrieveMultiple();
            return entityCollection.Entities.Count > 0;
        }
    }
}