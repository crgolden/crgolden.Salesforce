namespace Clarity.Salesforce
{
    public class SalesforceOptions
    {
        /// <summary>
        /// Force.com username
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Force.com password
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Force.com security token
        /// </summary>
        public string SecurityToken { get; set; }

        /// <summary>
        /// Consumer Key
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// Consumer Secret
        /// </summary>
        public string ClientSecret { get; set; }

        /// <summary>
        /// Force.com password + Force.com security token
        /// </summary>
        public string GrantType { get; set; } = "password";

        public string Url { get; set; }
    }
}
