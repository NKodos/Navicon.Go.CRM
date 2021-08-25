using System;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Navicon.Common.Entities;

namespace Navicon.Plugins.Communication.Handler
{
    public class CommunicationService
    {
        private readonly IOrganizationService _service;

        public CommunicationService(IOrganizationService organizationService)
        {
            _service = organizationService;
        }

        public void CheckCommunicationMain(new_communication targetCommunication)
        {
            var columns = new ColumnSet(new_communication.Fields.new_type,
                                                    new_communication.Fields.new_main,
                                                    new_communication.Fields.new_contactid);
            
            var currentCommunication = _service.Retrieve(new_communication.EntityLogicalName, 
                    targetCommunication.Id, columns)
                .ToEntity<new_communication>();

            var type = targetCommunication.new_type ?? currentCommunication.new_type;
            if (type == null) return;
            var main = targetCommunication.new_main ?? currentCommunication.new_main;
            var contactRef = targetCommunication.new_contactid ?? currentCommunication.new_contactid;

            if (main.GetValueOrDefault())
            {
                var query = new QueryExpression(new_communication.EntityLogicalName)
                {
                    ColumnSet = new ColumnSet(new_communication.Fields.Id),
                    NoLock = true,
                    TopCount = 1
                };
                query.Criteria.AddCondition(new_communication.Fields.new_contactid, ConditionOperator.Equal, contactRef.Id);
                query.Criteria.AddCondition(new_communication.Fields.new_type, ConditionOperator.Equal, (int)type.Value);

                var entityCollection = _service.RetrieveMultiple(query);
                if (entityCollection.Entities.Count > 0)
                {
                    throw new Exception("Основное средство связи с таким типом у выбранного контакта уже существует");
                }
            }
        }

        public void CheckNewCommunicationMain(new_communication targetCommunication)
        {
            if (targetCommunication.new_type == null ||
                targetCommunication.new_contactid == null ||
                targetCommunication.new_type == null) return;

            if (targetCommunication.new_main.GetValueOrDefault())
            {
                var query = new QueryExpression(new_communication.EntityLogicalName)
                {
                    ColumnSet = new ColumnSet(new_communication.Fields.Id),
                    NoLock = true,
                    TopCount = 1
                };
                query.Criteria.AddCondition(new_communication.Fields.new_contactid,
                    ConditionOperator.Equal, targetCommunication.new_contactid.Id);
                query.Criteria.AddCondition(new_communication.Fields.new_type,
                    ConditionOperator.Equal, (int)targetCommunication.new_type.Value);

                var entityCollection = _service.RetrieveMultiple(query);
                if (entityCollection.Entities.Count > 0)
                {
                    throw new Exception("Основное средство связи с таким типом у выбранного контакта уже существует");
                }
            }
        }
    }
}