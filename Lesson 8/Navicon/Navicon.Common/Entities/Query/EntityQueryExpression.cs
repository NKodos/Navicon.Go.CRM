using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace Navicon.Common.Entities.Query
{
    public class EntityQueryExpression
    {
        public QueryExpression Expression { get; set; }

        public IOrganizationService Service { get; private set; }

        public EntityQueryExpression(IOrganizationService service)
        {
            Service = service;
            Expression = new QueryExpression();
        }

        public EntityQueryExpression(IOrganizationService service, string entityLogicalName) : this(service)
        {
            Expression.EntityName = entityLogicalName;
        }

        public EntityQueryExpression(IOrganizationService service, string entityLogicalName, int topCount) : this (service, entityLogicalName)
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

        public void AddCondition(params ConditionExpression[] conditionExpression)
        {
            foreach (var expression in conditionExpression)
            {
                Expression.Criteria.AddCondition(expression);
            }
        }

        public EntityCollection RetrieveMultiple()
        {
            return Service.RetrieveMultiple(Expression);
        }
    }
}