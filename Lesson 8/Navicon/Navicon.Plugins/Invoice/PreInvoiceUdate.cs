using Microsoft.Extensions.DependencyInjection;
using Navicon.Common.Entities;
using Navicon.Plugins.Agreement.Handlers.Tools;
using Navicon.Plugins.Interfaces;
using Navicon.Plugins.Invoice.Handlers;

namespace Navicon.Plugins.Invoice
{
    public sealed class PreInvoiceUpdate : PluginBase<new_invoice>
    {
        public override void RegistrateServices(ServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IFactSummaTool, FactSummaTool>();
            serviceCollection.AddScoped<IService<new_invoice>, PreInvoiceUpdatingService>();
        }
    }
}