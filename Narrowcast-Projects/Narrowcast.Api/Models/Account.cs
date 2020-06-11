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

    public partial class Data
    {
        /// <summary>
        /// AccountID van Microsoft account.
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

    public partial class Headers
    {
        [JsonProperty("Content-Type")]
        public string ContentType { get; set; }
    }
}
