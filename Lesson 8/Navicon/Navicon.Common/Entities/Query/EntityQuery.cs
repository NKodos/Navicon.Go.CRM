using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace Navicon.Common.Entities.Query
{
    public class EntityQuery
    {
        public QueryExpression Expression { get; set; }

        public IOrganizationService Service { get; private set; }

        public EntityQuery(IOrganizationService service)
        {
            Service = service;
            Expression = new QueryExpression();
        }

        public EntityQuery(IOrganizationService service, string entityLogicalName) : this(service)
        {
            Expression.EntityName = entityLogicalName;
        }

        public EntityQuery(IOrganizationService service, string entityLogicalName, int topCount) : this (service, entityLogicalName)
        {
            Expression.TopCount = topCount;
        }

        public void AddColumns(params string[] columns)
        {
            Expression.ColumnSet = new ColumnSet(columns);
        }

        public void AddCondition(string attributeName, ConditionOperator conditionOperator, params object[] values)
        {
            Expression.Criteria.AddCondition(attributeName, conditionOperator, values);
        }

        public void AddCondition(ConditionExpression conditionExpression)
        {
            Expression.Criteria.AddCondition(conditionExpression);
        }

        public EntityCollection RetrieveMultiple()
        {
            return Service.RetrieveMultiple(Expression);
        }
    }
}