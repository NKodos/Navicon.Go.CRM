using Microsoft.Extensions.DependencyInjection;
using Navicon.Common.Entities;
using Navicon.Plugins.Interfaces;
using Navicon.Plugins.Invoice.Handlers;

namespace Navicon.Plugins.Invoice
{
    public sealed class PostInvoiceUpdate : PluginBase<new_invoice>
    {
        public override void RegistrateServices(ServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IService<new_invoice>, PostInvoiceCreationService>();
        }
    }
}