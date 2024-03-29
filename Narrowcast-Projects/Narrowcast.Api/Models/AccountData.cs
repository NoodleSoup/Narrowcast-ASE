using Newtonsoft.Json;

namespace Narrowcast.Api.Models
{
    public partial class AccountData
    {
        /// <summary>
        /// e-Mail from the teacher.
        /// </summary>
        /// <value>String</value>
        [JsonProperty("eMail")]
        public string eMail { get; set; } = "Unavailable";

        /// <summary>
        /// Phone number from the teacher (if present).
        /// </summary>
        /// <value>String</value>
        [JsonProperty("phoneNumber")]
        public string phoneNumber { get; set; } = "Unavailable";

        /// <summary>
        /// Whether the teacher is present.
        /// </summary>
        /// <value>String</value>
        [JsonProperty("teacherPresent")]
        public bool teacherPresent { get; set; } = false;

        /// <summary>
        /// If the teacher is reachable.
        /// </summary>
        /// <value></value>
        [JsonProperty("teacherReachable")]
        public bool teacherReachable { get; set; } = false;


        /// <summary>
        /// Account id in the database.
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