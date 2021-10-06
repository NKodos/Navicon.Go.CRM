using Navicon.Common.Entities;

namespace Navicon.Plugins.Interfaces.HandlersTools
{
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
}