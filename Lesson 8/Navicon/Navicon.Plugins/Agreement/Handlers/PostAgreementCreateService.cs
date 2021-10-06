using Microsoft.Xrm.Sdk;
using Navicon.Common.Entities;
using Navicon.Plugins.Agreement.Handlers.Tools;

namespace Navicon.Plugins.Agreement.Handlers
{
    public class PostAgreementCreateService : AgreementService
    {
        private readonly IFactTool _factTool;

        public PostAgreementCreateService(IOrganizationService service,
            IFactTool factTool) : base(service)
        {
            _factTool = factTool;
        }

        public override void Execute(new_agreement entity)
        {
            TryUpdateFact(entity);
        }

        private void TryUpdateFact(new_agreement targetAgreement)
        {
            var agreementResult = _factTool.TrySetFact(targetAgreement);

            if (agreementResult.Success)
            {
                Service.Update(agreementResult.Value);
            }
        }

    }
}