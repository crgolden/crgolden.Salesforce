namespace Clarity.Salesforce
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Shared;

    public abstract class DeleteRequestHandler<TModel, TRequest> : SalesforceRequestHandler, IRequestHandler<TRequest>
        where TModel : class
        where TRequest : DeleteRequest<TModel>
    {
        protected DeleteRequestHandler(IEnumerable<IIntegrationService> integrationServices) : base(integrationServices)
        {
        }

        public async Task<Unit> Handle(TRequest request, CancellationToken token)
        {
            await IntegrationService.Delete<TModel, TRequest>(request, token).ConfigureAwait(false);
            return Unit.Value;
        }
    }
}