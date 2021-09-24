using Microsoft.Extensions.DependencyInjection;
using Navicon.Common.Entities;
using Navicon.Plugins.Communication.Handler;
using Navicon.Plugins.Interfaces;

namespace Navicon.Plugins.Communication
{
    public sealed class PreCommunicationUpdate : PluginBase<new_communication>
    {
        public override void RegistrateServices(ServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IService<new_communication>, PreCommunicationUpdatingService>();
        }
    }
}