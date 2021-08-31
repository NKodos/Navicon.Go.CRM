using System.Collections.Generic;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace Navicon.Common.Entities.Query
{
    public class EntityQuery
    {
        public readonly string EntityName;

        public IOrganizationService Service { get; protected set; }

        public List<ConditionExpression> Conditions { get; protected set; }

        public ColumnSet ColumnSet { get; protected set; }

        public EntityQuery(IOrganizationService service, string entityName)
        {
            Service = service;
            EntityName = entityName;
            Conditions = new List<ConditionExpression>();
            ColumnSet = new ColumnSet();
        }

        public EntityQuery AddColumn(params string[] columnNames)
        {
            ColumnSet.AddColumns(columnNames);
            return this;
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
            var query = new EntityQueryExpression(Service, EntityName);
            query.AddCondition(Conditions);
            query.AddColumns(ColumnSet);
            return query.RetrieveMultiple();
        }

        public bool HasData()
        {
            var query = new EntityQueryExpression(Service, EntityName, 1);
            query.AddCondition(Conditions);

            var entityCollection = query.RetrieveMultiple();
            return entityCollection.Entities.Count > 0;
        }
    }
}