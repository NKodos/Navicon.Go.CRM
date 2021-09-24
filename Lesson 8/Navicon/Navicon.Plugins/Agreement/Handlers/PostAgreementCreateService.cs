using Microsoft.Xrm.Sdk;
using Navicon.Common.Entities;
using Navicon.Plugins.Agreement.Handlers.Tools;

namespace Navicon.Plugins.Agreement.Handlers
{
    public class PostAgreementCreateService : AgreementService
    {
        private readonly IAgreementFactTool _agreementFactTool;

        public PostAgreementCreateService(IOrganizationService service,
            IAgreementFactTool agreementFactTool) : base(service)
        {
            _agreementFactTool = agreementFactTool;
        }

        public override void Execute(new_agreement entity)
        {
            TryUpdateFact(entity);
        }

        private void TryUpdateFact(new_agreement targetAgreement)
        {
            var agreementResult = _agreementFactTool.TrySetFact(targetAgreement);

            if (agreementResult.Success)
            {
                Service.Update(agreementResult.Value);
            }
        }

    }
}