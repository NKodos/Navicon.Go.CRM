using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Navicon.TestConsole.Services;

namespace Navicon.TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            //const string connectionString = "AuthType=OAuth; " +
            //                                "Url=https://orgdf343115.crm4.dynamics.com/; " +
            //                                "Username=amin@august10t.onmicrosoft.com; " +
            //                                "Password=8753Navicon; " +
            //                                "RequireNewInstance=false; " +
            //                                "AppId=51f81489-12ee-4a9e-aaae-a2591f45987d; " +
            //                                "RedirectUri=app://58145B91-0C36-4500-8554-080854F2AC97; ";

            //var client = new CrmServiceClient(connectionString);
            //var service = (IOrganizationService)client;

            //var query = new QueryExpression(new_invoice.EntityLogicalName)
            //{
            //    ColumnSet = new ColumnSet(new_invoice.Fields.new_name, new_invoice.Fields.new_type),
            //    NoLock = true,
            //    TopCount = 20
            //};

            //var result = service.RetrieveMultiple(query);

            //foreach (var invoice in result.Entities.Select(e => e.ToEntity<new_invoice>()))
            //{
            //    Console.WriteLine($"{invoice.new_name}: {invoice.new_type}");
            //}

            //var eRef = new EntityReference();




            // Задача: раз в 2 секунды выдавать рандомное сообщение логов в консоль
            // Необходимо создать сервис, который будет это делать
            // Зарегистрировать сервис с помощью контейнера

            var host = CreateHost();
            host.Run();

            Console.ReadKey();
        }

        private static IHost CreateHost()
        {
            return CreateHostBuilder().Build();
        }

        private static IHostBuilder CreateHostBuilder()
        {
            return Host.CreateDefaultBuilder()
                .ConfigureServices((_, services) =>
                    services
                        .AddHostedService<Worker>()
                        .AddScoped<IRandomLogger, RandomLogger>());
        }
    }
}
