namespace crgolden.Salesforce
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Shared;

    public abstract class UpdateRequestHandler<TModel, TRequest> : SalesforceRequestHandler, IRequestHandler<TRequest>
        where TModel : class
        where TRequest : UpdateRequest<TModel>
    {
        protected UpdateRequestHandler(IEnumerable<IIntegrationService> integrationServices) : base(integrationServices)
        {
        }

        public async Task<Unit> Handle(TRequest request, CancellationToken token)
        {
            await IntegrationService.Update<TModel, TRequest>(request, token).ConfigureAwait(false);
            return Unit.Value;
        }
    }
}