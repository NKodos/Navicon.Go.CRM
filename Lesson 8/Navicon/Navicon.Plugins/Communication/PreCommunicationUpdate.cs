using Microsoft.Extensions.DependencyInjection;
using Navicon.Common.Entities;
using Navicon.Plugins.Communication.Handler;
using Navicon.Plugins.Communication.Handler.Tools;
using Navicon.Plugins.Interfaces;
using Navicon.Plugins.Interfaces.HandlersTools;

namespace Navicon.Plugins.Communication
{
    public sealed class PreCommunicationUpdate : PluginBase<new_communication>
    {
        public override void RegistrateServices(ServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<ICommunicationTool, CommunicationTool>();
            serviceCollection.AddScoped<IService<new_communication>, PreCommunicationUpdatingService>();
        }
    }
}