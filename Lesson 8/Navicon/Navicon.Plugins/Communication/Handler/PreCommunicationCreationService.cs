using Microsoft.Xrm.Sdk;
using Navicon.Common.Entities;
using Navicon.Plugins.Interfaces.HandlersTools;

namespace Navicon.Plugins.Communication.Handler
{
    public class PreCommunicationCreationService : CommunicationService
    {
        private readonly ICommunicationTool _communicationTool;

        public PreCommunicationCreationService(IOrganizationService organizationService,
            ICommunicationTool communicationTool) : base(organizationService)
        {
            _communicationTool = communicationTool;
        }

        public override void Execute(new_communication entity)
        {
            _communicationTool.CheckNewCommunicationMain(entity);
        }
    }
}