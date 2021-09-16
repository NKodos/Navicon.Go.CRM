using System;
using Microsoft.Xrm.Sdk;
using Navicon.Common.Entities;

namespace Navicon.Plugins.Communication.Handler
{
    public class PreCommunicationCreationService : CommunicationService
    {
        public PreCommunicationCreationService(IOrganizationService organizationService) : base(organizationService)
        {
        }

        public override void Execute(new_communication entity)
        {
            CheckNewCommunicationMain(entity);
        }

        /// <summary>
        /// Проверяет, что у контакта средства связи нет основного средства связи с текущим типом.
        /// Если существует - выбросит ошибку
        /// </summary>
        public void CheckNewCommunicationMain(new_communication targetCommunication)
        {
            if (targetCommunication.new_type == null ||
                targetCommunication.new_contactid == null) return;

            if (targetCommunication.new_main.GetValueOrDefault())
            {
                if (IsExistMainCommunication(targetCommunication.new_contactid.Id, targetCommunication.new_type.Value))
                {
                    throw new Exception("Основное средство связи с таким типом у выбранного контакта уже существует");
                }
            }
        }
    }
}