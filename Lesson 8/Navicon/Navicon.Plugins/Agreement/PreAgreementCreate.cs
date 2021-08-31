using Navicon.Common.Entities;
using Navicon.Plugins.Agreement.Handlers;

namespace Navicon.Plugins.Agreement
{
    public sealed class PreAgreementCreate : PluginBase<new_agreement>
    {
        public override void ExecuteBusinessLogics(ServiceInfo<new_agreement> serviceInfo)
        {
            var service = new AgreementService(serviceInfo.OrganizationService);
            service.UpdateContactFirstAgreementDate(serviceInfo.TargetEntity);
        }
    }
}