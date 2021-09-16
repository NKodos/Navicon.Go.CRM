using Navicon.Common.Entities;
using Navicon.Plugins.Communication.Handler;

namespace Navicon.Plugins.Communication
{
    public sealed class PreCommunicationUpdate : PluginBase<new_communication>
    {
        public override void ExecuteBusinessLogics(ServiceInfo<new_communication> serviceInfo)
        {
            var service = new PreCommunicationUpdatingService(serviceInfo.OrganizationService);
            service.Execute(serviceInfo.TargetEntity);
        }
    }
}