using System;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Navicon.Common.Entities;
using Navicon.Common.Entities.Query;
using Navicon.Plugins.Interfaces;

namespace Navicon.Plugins.Communication.Handler
{
    /// <summary>
    /// Сервис для работы с данными объекта Средство коммуникации
    /// </summary>
    public abstract class CommunicationService : IService<new_communication>
    {
        protected readonly IOrganizationService Service;

        protected CommunicationService(IOrganizationService organizationService)
        {
            Service = organizationService;
        }

        public abstract void Execute(new_communication entity);


        /// <summary>
        /// True - если уже существует главное средство связи с переданным типом у контакта
        /// </summary>
        protected bool IsExistMainCommunication(Guid contactId, new_communication_new_type type)
        {
            var query = new CommunicationQuery(Service);
            return query
                .AddCondition(
                    new ConditionExpression(new_communication.Fields.new_contactid, ConditionOperator.Equal, contactId), 
                    new ConditionExpression(new_communication.Fields.new_type, ConditionOperator.Equal, (int)type), 
                    new ConditionExpression(new_communication.Fields.new_main, ConditionOperator.Equal, true))
                .HasData();
        }
    }
}