namespace Clarity.Salesforce
{
    using System.Collections.Generic;
    using System.Linq;
    using Shared;

    public abstract class SalesforceRequestHandler
    {
        protected readonly IIntegrationService IntegrationService;

        protected SalesforceRequestHandler(IEnumerable<IIntegrationService> integrationServices)
        {
            IntegrationService = integrationServices.Single(x => x is SalesforceIntegrationService);
        }
    }
}
