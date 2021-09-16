using Navicon.Common.Entities;
using Navicon.Plugins.Communication.Handler;

namespace Navicon.Plugins.Communication
{
    public sealed class PreCommunicationCreate : PluginBase<new_communication>
    {
        public override void ExecuteBusinessLogics(ServiceInfo<new_communication> serviceInfo)
        {
            var service = new PreCommunicationCreationService(serviceInfo.OrganizationService);
            service.Execute(serviceInfo.TargetEntity);
        }
    }
}