using Newtonsoft.Json;

namespace Narrowcast.Api.Models
{
    public partial class Account
    {
        [JsonProperty("data")]
        public Data data { get; set; }

        [JsonProperty("headers")]
        public Headers headers { get; set; }
    }

    /// <summary>
    /// Data from the post request from the client.
    /// </summary>
    public partial class Data
    {
        /// <summary>
        /// AccountID of Microsoft account.
        /// </summary>
        /// <value>String</value>
        [JsonProperty("id")]
        public string id { get; set; }

        /// <summary>
        /// Account type: Student, Docent, Teamleider
        /// </summary>
        /// <value>String</value>
        [JsonProperty("type")]
        public string type { get; set; }
    }

    /// <summary>
    /// Headers from the post request.
    /// </summary>
    public partial class Headers
    {
        [JsonProperty("Content-Type")]
        public string ContentType { get; set; }
    }
}
