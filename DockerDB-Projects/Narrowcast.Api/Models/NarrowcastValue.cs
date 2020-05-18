namespace Narrowcast.Api.Models
{
    /// <summary>
    /// Class to initialize the container values Object
    /// </summary>
    public class NarrowcastValue
    {
        /// <summary>
        /// Primary key van de database die gekoppeld is aan de
        /// applicatie.
        /// </summary>
        /// <value>Integer</value>
        public int narrowcastNr { get; set; }

        /// <summary>
        /// Voornaam van de docent.
        /// </summary>
        /// <value>String</value>
        public string teacherFirst { get; set; }

        /// <summary>
        /// Achternaam van de docent
        /// </summary>
        /// <value>String</value>
        public string teacherLast { get; set; }

        /// <summary>
        /// Naam van de opleiding.
        /// </summary>
        /// <value>String</value>
        public string courseName { get; set; }

        /// <summary>
        /// ID van de opleiding.
        /// </summary>
        /// <value>String</value>
        public string courseID { get; set; }

        /// <summary>
        /// e-Mail van de docent.
        /// </summary>
        /// <value>String</value>
        public string eMail { get; set; }

        /// <summary>
        /// Telefoon nummer van de docent (mits aanwezig).
        /// </summary>
        /// <value>String</value>
        public string phoneNumber { get; set; }

        /// <summary>
        /// Of de docent aanwezig is of niet.
        /// </summary>
        /// <value>String</value>
        public bool teacherPresent { get; set; }

        /// <summary>
        /// Waar de leraar is in het gebouw.
        /// </summary>
        /// <value>String</value>
        public string classLocation { get; set; }

        /// <summary>
        /// Of de docent bereikbaar is of niet.
        /// </summary>
        /// <value></value>
        public bool teacherReachable { get; set; }

        /// <summary>
        /// Datum wanneer de docent data is geupdate. Formaat 'yyyy-MM-dd H:m:s'.
        /// </summary>
        /// <value>String</value>
        public string updateDate { get; set; }
    }

    /// <summary>
    /// Class to initialize the services value Object
    /// </summary>
    public class CoursesValue
    {
        /// <summary>
        /// Projectnaam op TeamCity waar de build configuraties instaan.
        /// </summary>
        /// <value>String</value>
        public string courseName { get; set; }
    }
}