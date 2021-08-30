using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace Navicon.Common.Entities.Query
{
    public class AgreementQuery
    {
        public const string EntityName = new_agreement.EntityLogicalName;

        public IOrganizationService Service { get; private set; }

        public AgreementQuery(IOrganizationService service)
        {
            Service = service;
        }

        public bool HasData(ConditionExpression conditionExpression = null)
        {
            var query = new EntityQuery(Service, EntityName, 1);

            if (conditionExpression != null)
            {
                query.AddCondition(conditionExpression);
            }

            var entityCollection = query.RetrieveMultiple();

            return entityCollection.Entities.Count > 0;
        }
    }
}