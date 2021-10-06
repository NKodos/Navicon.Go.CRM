using System;
using Microsoft.Xrm.Sdk;
using Navicon.Common.Entities;
using Navicon.Plugins.Interfaces;

namespace Navicon.Plugins.Agreement.Handlers
{
    /// <summary>
    /// Сервис для работы с договорами при Pre Operation
    /// </summary>
    public abstract class AgreementService : IService<new_agreement>
    {
        protected readonly IOrganizationService Service;

        public abstract void Execute(new_agreement entity);

        protected AgreementService(IOrganizationService service)
        {
            Service = service ?? throw new ArgumentNullException(nameof(service));
        }
    }
}