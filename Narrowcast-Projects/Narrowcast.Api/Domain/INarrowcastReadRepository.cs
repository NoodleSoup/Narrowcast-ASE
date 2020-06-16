using Narrowcast.Api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Narrowcast.Api.Domain
{
    /// <summary>
    /// Interface to make the methods accessible to other classes.
    /// </summary>
    public interface INarrowcastReadRepository
    {
        /// <summary>
        /// Endpoint to return all data or all data found by the specific search results
        /// </summary>
        /// <param name="teacherFirst"></param>
        /// <param name="teacherLast"></param>
        /// <param name="courseName"></param>
        /// <param name="teacherReachable"></param>
        /// <param name="updateDate"></param>
        /// <returns>Image/Container data in JSON</returns>
        Task<IEnumerable<NarrowcastValue>> CustomNarrowcastSearch(string teacherFirst,
                                                                string teacherLast,
                                                                string courseName,
                                                                string teacherReachable,
                                                                string updateDate);

        /// <summary>
        /// Endpoint to get all services
        /// </summary>
        /// <returns>All services once in JSON</returns>
        Task<IEnumerable<CoursesValue>> GetCourses();

        /// <summary>
        /// Endpoint to get the image/container info by specific service
        /// </summary>
        /// <param name="course"></param>
        /// <returns>Image/Container info per service</returns>
        Task<IEnumerable<NarrowcastValue>> GetNarrowcastByCourse(string course);

        /// <summary>
        /// Add account ID into the DB
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="accountType"></param>
        /// <returns>
        /// int of affected rows
        /// </returns>
        Task<int> AddIdToDb(string accountId, string accountType);

        /// <summary>
        /// Add account ID into the DB
        /// </summary>
        /// <param name="id"></param>
        /// <returns>
        /// int of affected rows
        /// </returns>
        Task<string> GetAccountType(string id);

        /// <summary>
        /// Add account ID into the DB
        /// </summary>
        /// <param name="id"></param>
        /// <returns>
        /// int of affected rows
        /// </returns>
        Task<AccountData> GetAccountData(string id);

        /// <summary>
        /// Add account data into the DB
        /// </summary>
        /// <param name="eMail"></param>
        /// <param name="phoneNumber"></param>
        /// <param name="teacherPresent"></param>
        /// <param name="teacherReachable"></param>
        /// <param name="id"></param>
        /// <returns>
        /// int of affected rows
        /// </returns>
        Task<int> SetAccountData(string eMail, string phoneNumber, bool teacherPresent, bool teacherReachable, string id);
    }
}