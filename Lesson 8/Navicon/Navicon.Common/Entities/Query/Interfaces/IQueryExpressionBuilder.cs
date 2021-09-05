using System.Collections.Generic;
using Microsoft.Xrm.Sdk.Query;

namespace Navicon.Common.Entities.Query.Interfaces
{
    public interface IQueryExpressionBuilder
    {
        IQueryExpressionBuilder AddColumns(ColumnSet columnSet);

        IQueryExpressionBuilder AddCondition(List<ConditionExpression> conditions);

        IQueryExpressionBuilder ClearColumns();

        IQueryExpressionBuilder ClearConditions();

        QueryBase Build();
    }
}