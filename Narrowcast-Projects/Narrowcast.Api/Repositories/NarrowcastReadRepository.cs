using Dapper;
using Narrowcast.Api.Domain;
using Narrowcast.Api.Models;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Narrowcast.Api.Repositories
{
    /// <summary>
    /// Contains all the queries used to obtain data from the database
    /// </summary>
    public class NarrowcastReadRepository : INarrowcastReadRepository
    {
        private readonly IDbConnection _connection;

        /// <summary>
        /// Constructor, cast DB connection to readonly DB connection.
        /// </summary>
        /// <param name="connection"></param>
        public NarrowcastReadRepository(IDbConnection connection)
            => _connection = connection;

        /// <summary>
        /// Returns all data based on the given variables
        /// if there are no variables given it defaults to return all values
        /// </summary>
        /// <example>
        /// <code>
        /// CustomContainerSearch(%,version-42",%,%,%);
        /// </code>
        /// </example>
        /// <param name="teacherFirst"></param>
        /// <param name="teacherLast"></param>
        /// <param name="courseName"></param>
        /// <param name="teacherReachable"></param>
        /// <param name="updateDate"></param>
        /// <returns>
        /// All data the database has on an image/container
        /// </returns>
        public async Task<IEnumerable<NarrowcastValue>> CustomNarrowcastSearch(string teacherFirst, string teacherLast, string courseName, string teacherReachable, string updateDate)
        {
            var result = await _connection.QueryAsync<NarrowcastValue>(
                @"SELECT *
                FROM narrowcast
                WHERE
                    teacherFirst LIKE @teacherFirst AND
                    teacherLast LIKE @teacherLast AND
                    courseName LIKE @courseName AND
                    teacherReachable LIKE @teacherReachable AND
                    updateDate >= @updateDate
                ORDER BY updateDate DESC",
                new { teacherFirst, teacherLast, courseName, teacherReachable, updateDate });

            return result.ToList();
        }

        /// <summary>
        /// Query to return all services in the database
        /// </summary>
        /// <returns>
        /// All services in the database
        /// if there's more then one it returns the service once
        /// </returns>
        public async Task<IEnumerable<CoursesValue>> GetCourses()
        {
            var result = await _connection.QueryAsync<CoursesValue>(
                @"SELECT courseName
                FROM narrowcast
                GROUP BY courseName;");

            return result;
        }

        /// <summary>
        /// Query to return all image/container data it has on a specific service
        /// </summary>
        /// <param name="course"></param>
        /// <returns>
        /// Image/Container data
        /// </returns>
        public async Task<IEnumerable<NarrowcastValue>> GetNarrowcastByCourse(string course)
        {
            var result = await _connection.QueryAsync<NarrowcastValue>(
                @"SELECT *
                FROM narrowcast
                WHERE courseName = @course
                ORDER BY updateDate DESC;",
                new { course });

            return result;
        }
    }
}