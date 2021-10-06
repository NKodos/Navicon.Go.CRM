using System;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Navicon.Common.Entities;
using Navicon.Common.Entities.Query;

namespace Navicon.Plugins.Communication.Handler.Tools
{
    // TODO: Вынести в общую папку Interfaces > Tools
    public interface ICommunicationTool
    {
        /// <summary>
        /// Проверяет существует ли основное средство связи у выбранного контакта
        /// с выбранным типом (E-mail / Телефон)
        /// </summary>
        void CheckCommunicationMain(new_communication targetCommunication);

        /// <summary>
        /// Проверяет, что у контакта средства связи нет основного средства связи с текущим типом.
        /// Если существует - выбросит ошибку
        /// </summary>
        void CheckNewCommunicationMain(new_communication targetCommunication);
    }

    public sealed class CommunicationTool : ICommunicationTool
    {
        private readonly IOrganizationService _service;

        public CommunicationTool(IOrganizationService service)
        {
            _service = service;
        }

        /// <summary>
        /// Проверяет существует ли основное средство связи у выбранного контакта
        /// с выбранным типом (E-mail / Телефон)
        /// </summary>
        public void CheckCommunicationMain(new_communication targetCommunication)
        {
            var columns = new ColumnSet(new_communication.Fields.new_type,
                new_communication.Fields.new_main,
                new_communication.Fields.new_contactid);

            var currentCommunication = _service.Retrieve(new_communication.EntityLogicalName,
                    targetCommunication.Id, columns)
                .ToEntity<new_communication>();

            var communivationType = targetCommunication.new_type ?? currentCommunication.new_type;
            if (communivationType == null) return;
            var main = targetCommunication.new_main ?? currentCommunication.new_main;
            var contactRef = targetCommunication.new_contactid ?? currentCommunication.new_contactid;

            if (main.GetValueOrDefault() &&
                IsExistMainCommunication(contactRef.Id, communivationType.Value))
            {
                throw new Exception("Основное средство связи с таким типом у выбранного контакта уже существует");
            }
        }

        /// <summary>
        /// Проверяет, что у контакта средства связи нет основного средства связи с текущим типом.
        /// Если существует - выбросит ошибку
        /// </summary>
        public void CheckNewCommunicationMain(new_communication targetCommunication)
        {
            if (targetCommunication.new_type == null ||
                targetCommunication.new_contactid == null) return;

            if (targetCommunication.new_main.GetValueOrDefault() &&
                IsExistMainCommunication(targetCommunication.new_contactid.Id,
                    targetCommunication.new_type.Value))
            {
                throw new Exception("Основное средство связи с таким типом у выбранного " +
                                    "контакта уже существует");
            }
        }

        /// <summary>
        /// True - если уже существует главное средство связи с переданным типом у контакта
        /// </summary>
        private bool IsExistMainCommunication(Guid contactId, new_communication_new_type type)
        {
            var query = new CommunicationQuery(_service);
            return query
                .AddCondition(
                    new ConditionExpression(new_communication.Fields.new_contactid, 
                        ConditionOperator.Equal, contactId),
                    new ConditionExpression(new_communication.Fields.new_type, 
                        ConditionOperator.Equal, (int)type),
                    new ConditionExpression(new_communication.Fields.new_main, 
                        ConditionOperator.Equal, true))
                .HasData();
        }
    }
}