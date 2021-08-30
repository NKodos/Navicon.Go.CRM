﻿using Microsoft.Xrm.Sdk;

namespace Navicon.Common.Entities.Query
{
    public class AgreementQuery : EntityQuery
    {
        public AgreementQuery(IOrganizationService service) : base(service, new_agreement.EntityLogicalName)
        {

        }
    }
}