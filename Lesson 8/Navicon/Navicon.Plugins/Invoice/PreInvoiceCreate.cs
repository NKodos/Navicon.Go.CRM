using Microsoft.Extensions.DependencyInjection;
using Navicon.Common.Entities;
using Navicon.Plugins.Agreement.Handlers.Tools;
using Navicon.Plugins.Interfaces;
using Navicon.Plugins.Invoice.Handlers;
using Navicon.Plugins.Invoice.Handlers.Tools;

namespace Navicon.Plugins.Invoice
{
    public sealed class PreInvoiceCreate : PluginBase<new_invoice>
    {
        public override void RegistrateServices(ServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IFactSummaTool, FactSummaTool>();
            serviceCollection.AddScoped<IService<new_invoice>, PreInvoiceCreationService>();
        }
    }
}