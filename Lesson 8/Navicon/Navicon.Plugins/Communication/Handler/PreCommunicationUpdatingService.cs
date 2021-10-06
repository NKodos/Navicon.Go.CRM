using Microsoft.Xrm.Sdk;
using Navicon.Common.Entities;
using Navicon.Plugins.Interfaces.HandlersTools;

namespace Navicon.Plugins.Communication.Handler
{
    public class PreCommunicationUpdatingService : CommunicationService
    {
        private readonly ICommunicationTool _communicationTool;

        public PreCommunicationUpdatingService(IOrganizationService organizationService,
            ICommunicationTool communicationTool) : base(organizationService)
        {
            _communicationTool = communicationTool;
        }

        public override void Execute(new_communication entity)
        {
            _communicationTool.CheckCommunicationMain(entity);
        }
    }
}