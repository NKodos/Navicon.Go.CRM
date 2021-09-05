using Microsoft.Xrm.Sdk.Query;

namespace Navicon.Common.Extension
{
    public static class QueryExpressionExtension
    {
        public static QueryExpression AddColumns(this QueryExpression expression, ColumnSet columnSet)
        {
            expression.ColumnSet = columnSet;
            return expression;
        }
        
        public static QueryExpression AddColumns(this QueryExpression expression, params string[] columns)
        {
            return expression.AddColumns(new ColumnSet(columns));
        }
    }
}