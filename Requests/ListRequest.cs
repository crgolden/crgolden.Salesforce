namespace Clarity.Salesforce
{
    using System;
    using System.Collections.Generic;
    using MediatR;

    public abstract class ListRequest<TModel> : SalesforceRequest, IRequest<TModel[]>
        where TModel : class
    {
        public readonly DateTime? CompareDate;

        public readonly string CompareField;

        public readonly IEnumerable<string> Fields;

        public readonly string Query;

        protected ListRequest(
            DateTime? compareDate,
            IEnumerable<string> fields,
            string compareField = "LastModifiedDate",
            string query = null)
        {
            CompareDate = compareDate;
            CompareField = compareField;
            Fields = fields;
            Query = query;
        }
    }
}
