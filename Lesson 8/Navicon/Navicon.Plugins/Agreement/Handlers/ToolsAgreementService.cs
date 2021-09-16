using Microsoft.Xrm.Sdk;
using Navicon.Common.Entities;

namespace Navicon.Plugins.Agreement.Handlers
{
    // Todo: временный класс. Используется сервисами других сущностей. Необходимо убрать!!!
    public class ToolsAgreementService : AgreementService
    {
        public ToolsAgreementService(IOrganizationService service) : base(service)
        {
        }

        public override void Execute(new_agreement entity)
        {
            
        }
    }
}