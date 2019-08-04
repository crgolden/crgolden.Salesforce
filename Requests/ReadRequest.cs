namespace crgolden.Salesforce
{
    using System.Collections.Generic;
    using MediatR;

    public abstract class ReadRequest<TModel> : SalesforceRequest, IRequest<TModel>
        where TModel : class
    {
        public readonly string Id;

        public readonly IEnumerable<string> Fields;

        protected ReadRequest(string id, IEnumerable<string> fields)
        {
            Id = id;
            Fields = fields;
        }
    }
}