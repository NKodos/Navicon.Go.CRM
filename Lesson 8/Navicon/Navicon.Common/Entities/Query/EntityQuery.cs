using System.Collections.Generic;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Navicon.Common.Entities.Query.Interfaces;

namespace Navicon.Common.Entities.Query
{
    public class EntityQuery
    {
        public readonly string EntityName;

        public IOrganizationService Service { get; protected set; }

        public IQueryExpressionBuilder ExpressionBuilder { get; protected set; }

        public List<ConditionExpression> Conditions { get; protected set; }

        public ColumnSet ColumnSet { get; protected set; }

        public EntityQuery(IOrganizationService service, string entityName)
        {
            Service = service;
            EntityName = entityName;
            Conditions = new List<ConditionExpression>();
            ColumnSet = new ColumnSet();
            ExpressionBuilder = new QueryExpressionBuilder();
        }

        public EntityQuery AddCondition(params ConditionExpression[] conditions)
        {
            foreach (var condition in conditions)
            {
                Conditions.Add(condition);
            }

            return this;
        }

        public EntityQuery ClearColumns()
        {
            ColumnSet = new ColumnSet();
            return this;
        }

        public EntityQuery ClearConditions()
        {
            Conditions.Clear();
            return this;
        }

        public EntityCollection GetAll()
        {
            var query = CreateNewExpression();
            return Service.RetrieveMultiple(query);
        }

        public bool HasData()
        {
            var query = CreateNewExpression();
            var entityCollection = Service.RetrieveMultiple(query);

            return entityCollection.Entities.Count > 0;
        }

        protected QueryBase CreateNewExpression()
        {
            return ExpressionBuilder
                .ClearColumns()
                .ClearConditions()
                .AddColumns(ColumnSet)
                .AddCondition(Conditions)
                .Build();
        }
    }
}