using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using Moq;
using Navicon.Plugins.Invoice.Handlers;
using Microsoft.Extensions.DependencyInjection;
using Navicon.Common.Entities;
using Navicon.Plugins.Interfaces;
using Navicon.Plugins.Invoice;
using Navicon.Plugins.Invoice.Handlers.Tools;

namespace Navicon.Tests
{
    [TestClass]
    public class InvoiceServiceTests2
    {
        private readonly Mock<IOrganizationService> _serviceMock = new Mock<IOrganizationService>();

        [TestMethod]
        public void TestInvoiceService()
        {
            var serviceProvider = Build(_serviceMock.Object);

            var invoiceService = serviceProvider.GetService<IService<new_invoice>>();
            invoiceService?.Execute(new new_invoice { new_fact = true });
        }
        //
        // [TestMethod]
        // public void TestPlugin()
        // {
        //     var plugin = new PostInvoiceCreate();
        //     plugin.ExecuteTest(_serviceMock.Object, new new_invoice());
        // }

        public IServiceProvider Build(IOrganizationService organizationService)
        {
            var container = new ServiceCollection();

            container.AddScoped<IPayDateTool, PayDateTool>();
            container.AddScoped<IService<new_invoice>, PostInvoiceCreationService>(x =>
                new PostInvoiceCreationService(organizationService, x.GetService<IPayDateTool>()));

            return container.BuildServiceProvider();
        }
    }
}