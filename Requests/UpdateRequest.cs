namespace crgolden.Salesforce
{
    using MediatR;

    public abstract class UpdateRequest<TModel> : IRequest
    {
        public readonly string Id;

        public readonly TModel Model;

        protected UpdateRequest(string id, TModel model)
        {
            Id = id;
            Model = model;
        }
    }
}
