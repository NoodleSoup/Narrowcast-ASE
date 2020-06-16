using Newtonsoft.Json;

namespace Narrowcast.Api.Models
{
    public partial class AccountData
    {
        /// <summary>
        /// e-Mail van de docent.
        /// </summary>
        /// <value>String</value>
        [JsonProperty("eMail")]
        public string eMail { get; set; } = "Unavailable";

        /// <summary>
        /// Telefoon nummer van de docent (mits aanwezig).
        /// </summary>
        /// <value>String</value>
        [JsonProperty("phoneNumber")]
        public string phoneNumber { get; set; } = "Unavailable";

        /// <summary>
        /// Of de docent aanwezig is of niet.
        /// </summary>
        /// <value>String</value>
        [JsonProperty("teacherPresent")]
        public bool teacherPresent { get; set; } = false;

        /// <summary>
        /// Of de docent bereikbaar is of niet.
        /// </summary>
        /// <value></value>
        [JsonProperty("teacherReachable")]
        public bool teacherReachable { get; set; } = false;


        /// <summary>
        /// ID van het account in de database.
        /// </summary>
        /// <value></value>
        [JsonProperty("accountId")]
        public string accountId { get; set; } = null;
    }

    public partial class AccountDataPost
    {
        [JsonProperty("data")]
        public AccountData data { get; set; }

        [JsonProperty("headers")]
        public Headers headers { get; set; }
    }
}