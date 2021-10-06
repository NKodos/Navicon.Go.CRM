using Microsoft.Extensions.DependencyInjection;
using Navicon.Common.Entities;
using Navicon.Plugins.Agreement.Handlers;
using Navicon.Plugins.Agreement.Handlers.Tools;
using Navicon.Plugins.Interfaces;

namespace Navicon.Plugins.Agreement
{
    public sealed class PostAgreementCreate : PluginBase<new_agreement>
    {
        public override void RegistrateServices(ServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IFactTool, FactTool>();
            serviceCollection.AddScoped<IService<new_agreement>, PostAgreementCreateService>();
        }
    }
}