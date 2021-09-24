using Microsoft.Extensions.DependencyInjection;
using Navicon.Common.Entities;
using Navicon.Plugins.Interfaces;
using Navicon.Plugins.Invoice.Handlers;
using Navicon.Plugins.Invoice.Handlers.Tools;

namespace Navicon.Plugins.Invoice
{
    public sealed class PostInvoiceCreate : PluginBase<new_invoice>
    {
        public override void RegistrateServices(ServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IPayDateTool, PayDateTool>();
            serviceCollection.AddScoped<IService<new_invoice>, PostInvoiceCreationService>();
        }
    }
}