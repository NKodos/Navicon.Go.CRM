using Navicon.Common.Entities;
using Navicon.Plugins.Agreement.Handlers;

namespace Navicon.Plugins.Agreement
{
    public sealed class PostAgreementCreate : PluginBase<new_agreement>
    {
        public override void ExecuteBusinessLogics(ServiceInfo<new_agreement> serviceInfo)
        {
            var service = new PostAgreementCreateService(serviceInfo.OrganizationService);
            service.Execute(serviceInfo.TargetEntity);
        }
    }
}