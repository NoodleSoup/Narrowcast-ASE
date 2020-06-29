using System.Text.Json;

namespace Narrowcast.Api.Logging
{
    /// <summary>
    /// Class for returning custom error layout.
    /// </summary>
    public class ErrorDetails
    {
        /// <summary>
        /// Web statuscode of the error
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// Message displayed as reason for the error.
        /// </summary>
        public string Message { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
