namespace Clarity.Salesforce
{
    using MediatR;

    public abstract class DeleteRequest<TModel> : SalesforceRequest, IRequest
        where TModel : class
    {
        public readonly string Id;

        protected DeleteRequest(string id)
        {
            Id = id;
        }
    }
}
