namespace crgolden.Salesforce
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Shared;

    public abstract class ReadRequestHandler<TModel, TRequest> : SalesforceRequestHandler, IRequestHandler<TRequest, TModel>
        where TModel : class
        where TRequest : ReadRequest<TModel>
    {
        protected ReadRequestHandler(IEnumerable<IIntegrationService> integrationServices) : base(integrationServices)
        {
        }

        public virtual async Task<TModel> Handle(TRequest request, CancellationToken token)
        {
            return await IntegrationService.Read<TModel, TRequest>(request, token).ConfigureAwait(false);
        }
    }
}
