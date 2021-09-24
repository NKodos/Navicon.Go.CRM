using Microsoft.Xrm.Sdk;

namespace Navicon.Common.Entities.Query
{
    public class CommunicationQuery : EntityQuery<new_communication>
    {
        public CommunicationQuery(IOrganizationService service) : base(service, new_communication.EntityLogicalName)
        {
        }
    }
}