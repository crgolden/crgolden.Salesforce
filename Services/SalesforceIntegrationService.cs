namespace Clarity.Salesforce
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web;
    using Microsoft.Extensions.Options;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using Shared;

    public class SalesforceIntegrationService : IIntegrationService
    {
        private const string TokenEndpoint = "https://login.salesforce.com/services/oauth2/token";
        private readonly HttpClient _httpClient;
        private readonly Dictionary<string, string> _keyValuePairs;
        private string _instanceUrl;

        public SalesforceIntegrationService(
            IHttpClientFactory httpClientFactory,
            IOptions<SalesforceOptions> salesforceOptions)
        {
            _httpClient = httpClientFactory.CreateClient(nameof(SalesforceIntegrationService));
            _keyValuePairs = new Dictionary<string, string>
            {
                {"grant_type", salesforceOptions.Value.GrantType},
                {"client_id", salesforceOptions.Value.ClientId},
                {"client_secret", salesforceOptions.Value.ClientSecret},
                {"username", salesforceOptions.Value.Username},
                {"password", $"{salesforceOptions.Value.Password}{salesforceOptions.Value.SecurityToken}"}
            };
        }

        public async Task<TModel[]> List<TModel, TRequest>(TRequest request, CancellationToken token) where TModel : class
        {
            if (!(request is ListRequest<TModel> listRequest)) throw new ArgumentException("Invalid request");
            await Login(token).ConfigureAwait(false);
            var records = new List<TModel>();

            string GetQuery()
            {
                var query = HttpUtility.UrlEncode($"Select {string.Join(",", listRequest.Fields)} From {typeof(TModel).Name}");
                if (!listRequest.CompareDate.HasValue) return query;
                var compareDate = $"{listRequest.CompareDate.Value:s}Z";
                query += HttpUtility.UrlEncode($" Where {listRequest.CompareField} > {compareDate}");

                return query;
            }

            async Task GetRecords(string requestUri)
            {
                SalesforceListResponse<TModel> salesforceResponse;
                using (var response = await _httpClient.GetAsync(requestUri, token).ConfigureAwait(false))
                {
                    response.EnsureSuccessStatusCode();
                    var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    salesforceResponse = JsonConvert.DeserializeObject<SalesforceListResponse<TModel>>(responseString);
                }

                records.AddRange(salesforceResponse.Records);
                if (!salesforceResponse.Done) await GetRecords($"{_instanceUrl}{salesforceResponse.NextRecordsUrl}").ConfigureAwait(false);
            }

            await GetRecords($"{_httpClient.BaseAddress}/query?q={listRequest.Query ?? GetQuery()}").ConfigureAwait(false);
            return records.ToArray();
        }

        public async Task<TModel> Create<TModel, TRequest>(TRequest request, CancellationToken token) where TModel : class
        {
            if (!(request is CreateRequest<TModel> createRequest)) throw new ArgumentException("Invalid request");
            await Login(token).ConfigureAwait(false);
            var requestUri = $"{_httpClient.BaseAddress}/sobjects/{typeof(TModel).Name}";
            SalesforceCreateResponse salesforceResponse;
            var model = JsonConvert.SerializeObject(createRequest.Model);
            using (var content = new StringContent(model, Encoding.UTF8, "application/json"))
            using (var response = await _httpClient.PostAsync(requestUri, content, token).ConfigureAwait(false))
            {
                response.EnsureSuccessStatusCode();
                var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                salesforceResponse = JsonConvert.DeserializeObject<SalesforceCreateResponse>(responseString);
            }

            var fields = typeof(TModel).GetProperties().Select(x => x.Name);
            requestUri = $"{_httpClient.BaseAddress}/sobjects/{typeof(TModel).Name}/{salesforceResponse.Id}?fields={string.Join(",", fields)}";
            using (var response = await _httpClient.GetAsync(requestUri, token).ConfigureAwait(false))
            {
                response.EnsureSuccessStatusCode();
                var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                return JsonConvert.DeserializeObject<TModel>(responseString);
            }
        }

        public async Task<TModel> Read<TModel, TRequest>(TRequest request, CancellationToken token) where TModel : class
        {
            if (!(request is ReadRequest<TModel> readRequest)) throw new ArgumentException("Invalid request");
            await Login(token).ConfigureAwait(false);
            var requestUri = $"{_httpClient.BaseAddress}/sobjects/{typeof(TModel).Name}/{readRequest.Id}?fields={string.Join(",", readRequest.Fields)}";
            using (var response = await _httpClient.GetAsync(requestUri, token).ConfigureAwait(false))
            {
                response.EnsureSuccessStatusCode();
                var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                return JsonConvert.DeserializeObject<TModel>(responseString);
            }
        }

        public async Task Update<TModel, TRequest>(TRequest request, CancellationToken token) where TModel : class
        {
            if (!(request is UpdateRequest<TModel> updateRequest)) throw new ArgumentException("Invalid request");
            await Login(token).ConfigureAwait(false);
            var requestUri = $"{_httpClient.BaseAddress}/sobjects/{typeof(TModel).Name}/{updateRequest.Id}";
            using (var content = new StringContent(JsonConvert.SerializeObject(updateRequest.Model), Encoding.UTF8, "application/json"))
            using (var message = new HttpRequestMessage(new HttpMethod("PATCH"), requestUri) { Content = content })
            using (var response = await _httpClient.SendAsync(message, token).ConfigureAwait(false))
            {
                response.EnsureSuccessStatusCode();
            }
        }

        public async Task Delete<TModel, TRequest>(TRequest request, CancellationToken token) where TModel : class
        {
            if (!(request is DeleteRequest<TModel> deleteRequest)) throw new ArgumentException("Invalid request");
            await Login(token).ConfigureAwait(false);
            var requestUri = $"{_httpClient.BaseAddress}/sobjects/{typeof(TModel).Name}/{deleteRequest.Id}";
            using (var response = await _httpClient.DeleteAsync(requestUri, token).ConfigureAwait(false))
            {
                response.EnsureSuccessStatusCode();
            }
        }

        private async Task Login(CancellationToken token)
        {
            using (var content = new FormUrlEncodedContent(_keyValuePairs))
            using (var response = await _httpClient.PostAsync(TokenEndpoint, content, token).ConfigureAwait(false))
            {
                response.EnsureSuccessStatusCode();
                var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var obj = JObject.Parse(responseString);
                _instanceUrl = $"{obj["instance_url"]}";
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                    scheme: "Bearer",
                    parameter: $"{obj["access_token"]}");
            }
        }

        private class SalesforceListResponse<T>
        {
            [JsonProperty("totalSize")]
            public string TotalSize { get; set; }

            [JsonProperty("done")]
            public bool Done { get; set; }

            [JsonProperty("nextRecordsUrl")]
            public string NextRecordsUrl { get; set; }

            [JsonProperty("records")]
            public T[] Records { get; set; }
        }

        private class SalesforceCreateResponse
        {
            [JsonProperty("id")]
            public string Id { get; set; }

            [JsonProperty("errors")]
            public string[] Errors { get; set; }

            [JsonProperty("success")]
            public bool Success { get; set; }
        }
    }
}
