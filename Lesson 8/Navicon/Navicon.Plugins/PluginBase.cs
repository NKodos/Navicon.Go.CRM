using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xrm.Sdk;
using Navicon.Plugins.Interfaces;

namespace Navicon.Plugins
{
    public abstract class PluginBase<T> : IPlugin where T: Entity
    {
        protected IServiceProvider Container { get; private set; }

        public abstract void RegistrateServices(ServiceCollection serviceCollection);

        public void Execute(IServiceProvider serviceProvider)
        {
            var serviceInfo = GetBaseInfo(serviceProvider);
            try
            {
                CreatePluginServiceProvider(serviceInfo);
                ExcecuteService(serviceInfo);
            }
            catch (Exception ex)
            {
                serviceInfo.TracingService.Trace(ex.ToString());
                throw new InvalidPluginExecutionException(ex.Message);
            }
        }

        private void CreatePluginServiceProvider(IServiceInfo<T> serviceInfo)
        {
            var container = new ServiceCollection();
            container.AddScoped(x => serviceInfo.OrganizationService);

            RegistrateServices(container);

            Container = container.BuildServiceProvider();
        }

        private void ExcecuteService(IServiceInfo<T> serviceInfo)
        {
            var service = Container.GetService<IService<T>>();
            if (service == null) throw new NullReferenceException("Сервис плагина не найден");

            service.Execute(serviceInfo.TargetEntity);
        }

        protected IServiceInfo<T> GetBaseInfo(IServiceProvider serviceProvider)
        {
            var info = new ServiceInfo<T>(serviceProvider);

            info.GetTracingService();
            info.TracingService.Trace("Получен Tracing Service");

            info.GetOrganizationService();
            info.TracingService.Trace("Получен Organization Service");

            info.GetTargetEntity();
            info.TracingService.Trace($"Получен Target Entity типа: {typeof(T)}");

            return info;
        }
    }
}