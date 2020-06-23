namespace Narrowcast.Api.Models
{
    /// <summary>
    /// Class to initialize the narrowcast values Object
    /// </summary>
    public class NarrowcastValue
    {
        /// <summary>
        /// Firstname of the teacher.
        /// </summary>
        /// <value>String</value>
        public string teacherFirst { get; set; }

        /// <summary>
        /// Lastname of the teacher.
        /// </summary>
        /// <value>String</value>
        public string teacherLast { get; set; }

        /// <summary>
        /// Name of the course.
        /// </summary>
        /// <value>String</value>
        public string courseName { get; set; }

        /// <summary>
        /// ID of the course.
        /// </summary>
        /// <value>String</value>
        public string courseID { get; set; }

        /// <summary>
        /// e-Mail from the teacher.
        /// </summary>
        /// <value>String</value>
        public string eMail { get; set; }

        /// <summary>
        /// Phone number from the teacher (if present).
        /// </summary>
        /// <value>String</value>
        public string phoneNumber { get; set; }

        /// <summary>
        /// Whether the teacher is present.
        /// </summary>
        /// <value>String</value>
        public bool teacherPresent { get; set; }

        /// <summary>
        /// Where the teacher is in the school.
        /// </summary>
        /// <value>String</value>
        public string classLocation { get; set; }

        /// <summary>
        /// If the teacher is reachable.
        /// </summary>
        /// <value></value>
        public bool teacherReachable { get; set; }

        /// <summary>
        /// Date of last update. Format 'yyyy-MM-dd H:m:s'.
        /// </summary>
        /// <value>String</value>
        public string updateDate { get; set; }
    }

    /// <summary>
    /// Class to initialize the courses value Object
    /// </summary>
    public class CoursesValue
    {
        /// <summary>
        /// Name of all courses available in the database.
        /// </summary>
        /// <value>String</value>
        public string courseName { get; set; }
    }
}