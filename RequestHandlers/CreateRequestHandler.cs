namespace crgolden.Salesforce
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Shared;

    public abstract class CreateRequestHandler<TModel, TRequest> : SalesforceRequestHandler, IRequestHandler<TRequest, TModel>
        where TModel : class
        where TRequest : CreateRequest<TModel>
    {
        protected CreateRequestHandler(IEnumerable<IIntegrationService> integrationServices) : base(integrationServices)
        {
        }

        public async Task<TModel> Handle(TRequest request, CancellationToken token)
        {
            return await IntegrationService.Create<TModel, TRequest>(request, token).ConfigureAwait(false);
        }
    }
}
