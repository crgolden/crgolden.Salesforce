namespace crgolden.Salesforce
{
    using MediatR;

    public abstract class CreateRequest<TModel> : SalesforceRequest, IRequest<TModel>
        where TModel : class
    {

        public readonly TModel Model;

        public CreateRequest(TModel model)
        {
            Model = model;
        }
    }
}
