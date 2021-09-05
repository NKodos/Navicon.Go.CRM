using System.Collections.Generic;
using Microsoft.Xrm.Sdk.Query;
using Navicon.Common.Entities.Query.Interfaces;

namespace Navicon.Common.Entities.Query
{
    public class QueryExpressionBuilder : IQueryExpressionBuilder
    {
        public List<ConditionExpression> Conditions { get; protected set; } = new List<ConditionExpression>();

        public ColumnSet ColumnSet { get; protected set; } = new ColumnSet();

        public IQueryExpressionBuilder AddColumns(ColumnSet columnSet)
        {
            ColumnSet = columnSet;
            return this;
        }

        public IQueryExpressionBuilder AddCondition(List<ConditionExpression> conditions)
        {
            Conditions = conditions;
            return this;
        }

        public IQueryExpressionBuilder ClearColumns()
        {
            ColumnSet = new ColumnSet();
            return this;
        }

        public IQueryExpressionBuilder ClearConditions()
        {
            Conditions.Clear();
            return this;
        }

        public QueryBase Build()
        {
            var expression = new QueryExpression
            {
                ColumnSet = ColumnSet
            };
            AddConditionsToExtression(expression);


            return expression;
        }

        private void AddConditionsToExtression(QueryExpression expression)
        {
            foreach (var condition in Conditions)
            {
                expression.Criteria.AddCondition(condition);
            }
        }
    }
}