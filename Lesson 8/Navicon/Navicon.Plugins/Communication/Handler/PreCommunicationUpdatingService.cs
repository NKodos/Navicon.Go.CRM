using System;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Navicon.Common.Entities;

namespace Navicon.Plugins.Communication.Handler
{
    public class PreCommunicationUpdatingService : CommunicationService
    {
        public PreCommunicationUpdatingService(IOrganizationService organizationService) : base(organizationService)
        {
        }

        public override void Execute(new_communication entity)
        {
            CheckCommunicationMain(entity);
        }

        /// <summary>
        /// Проверяет существует ли основное средство связи у выбранного контакта
        /// с выбранным типом (E-mail / Телефон)
        /// </summary>
        protected void CheckCommunicationMain(new_communication targetCommunication)
        {
            var columns = new ColumnSet(new_communication.Fields.new_type,
                new_communication.Fields.new_main,
                new_communication.Fields.new_contactid);

            var currentCommunication = Service.Retrieve(new_communication.EntityLogicalName,
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
    }
}