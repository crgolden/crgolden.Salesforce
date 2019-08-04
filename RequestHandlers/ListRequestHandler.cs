namespace crgolden.Salesforce
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Shared;

    public abstract class ListRequestHandler<TModel, TRequest> : SalesforceRequestHandler, IRequestHandler<TRequest, TModel[]>
        where TModel : class
        where TRequest : ListRequest<TModel>
    {
        protected ListRequestHandler(IEnumerable<IIntegrationService> integrationServices) : base(integrationServices)
        {
        }

        public virtual async Task<TModel[]> Handle(TRequest request, CancellationToken token)
        {
            return await IntegrationService.List<TModel, TRequest>(request, token).ConfigureAwait(false);
        }
    }
}