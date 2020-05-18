namespace Narrowcast.Api.Settings
{
    /// <summary>
    /// Class to get the Application settings.
    /// </summary>
    public class AppSettings
    {
        /// <summary>
        /// Request connection string from Database prefix.
        /// </summary>
        /// <value>DatabaseSettings</value>
        public static DatabaseSettings Database { get; set; }
    }
}
