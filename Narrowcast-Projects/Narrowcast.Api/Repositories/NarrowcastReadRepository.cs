using Dapper;
using Narrowcast.Api.Domain;
using Narrowcast.Api.Models;
using System;
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
        /// CustomNarrowcastSearch(%,"Foppele",%,%,%);
        /// </code>
        /// </example>
        /// <param name="teacherFirst"></param>
        /// <param name="teacherLast"></param>
        /// <param name="courseName"></param>
        /// <param name="teacherReachable"></param>
        /// <param name="updateDate"></param>
        /// <returns>
        /// All data the database has on a teacher
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
        /// Query to return all courses in the database
        /// </summary>
        /// <returns>
        /// All courses in the database
        /// if there's more then one it returns the course once
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
        /// Query to return all teacher data it has on a specific course
        /// </summary>
        /// <param name="course"></param>
        /// <returns>
        /// Teacher data
        /// </returns>
        public async Task<IEnumerable<NarrowcastValue>> GetNarrowcastByCourse(string course)
        {
            var result = await _connection.QueryAsync<NarrowcastValue>(
                @"SELECT
                        teacherFirst,
                        teacherLast,
                        courseName,
                        courseID,
                        eMail,
                        phoneNumber,
                        teacherPresent,
                        classLocation,
                        teacherReachable,
                        updateDate
                FROM narrowcast
                WHERE courseName = @course
                ORDER BY updateDate DESC;",
                new { course });

            return result;
        }

        /// <summary>
        /// Add account ID into the DB
        /// </summary>
        /// <param name="id"></param>
        /// <returns>
        /// int of affected rows
        /// </returns>
        public async Task<string> GetAccountType(string id)
        {
            var result = await _connection.QuerySingleAsync<string>(
                @"SELECT accountType
                FROM AccountIDs
                WHERE accountId = @id;",
                new { id });

            return result;
        }

        /// <summary>
        /// Add account ID into the DB
        /// </summary>
        /// <param name="id"></param>
        /// <param name="accountType"></param>
        /// <returns>
        /// int of affected rows
        /// </returns>
        public async Task<int> AddIdToDb(string id, string accountType)
        {
            try
            {
                var result = await _connection.ExecuteAsync(
                    @"INSERT INTO
                            AccountIDs (accountId, accountType)
                    VALUES (@id, @accountType);",
                    new { id, accountType });

                return result;
            }
            catch(MySql.Data.MySqlClient.MySqlException eSql)
            {
                System.Diagnostics.Debug.WriteLine($"SQL Error: {eSql.Message} in {eSql.Source}");
                return 0;
            }
        }

        /// <summary>
        /// Get account from the DB
        /// </summary>
        /// <param name="id"></param>
        /// <returns>
        /// account data
        /// </returns>
        public async Task<AccountData> GetAccountData(string id)
        {
            try
            {
                var result = await _connection.QuerySingleAsync<AccountData>(
                    @"SELECT eMail,
                        phoneNumber,
                        teacherPresent,
                        teacherReachable
                    FROM narrowcast
                    WHERE account_id in (
                        SELECT accountId
                        FROM accountids
                        WHERE accountType = 'teacher'
                        AND accountId = @id
                    );",
                    new { id });

                return result;
            }
            catch(System.InvalidOperationException ioe)
            {
                System.Diagnostics.Debug.WriteLine($"Error Message: {ioe.Message} in {ioe.Source} at {DateTime.Now:yyyy-MM-dd H:mm:ss}");
                return new AccountData();

            }
        }

        /// <summary>
        /// Update account data into the DB
        /// </summary>
        /// <param name="eMail"></param>
        /// <param name="phoneNumber"></param>
        /// <param name="teacherPresent"></param>
        /// <param name="teacherReachable"></param>
        /// <param name="id"></param>
        /// <returns>
        /// int of affected rows
        /// </returns>
        public async Task<int> SetAccountData(string eMail, string phoneNumber, bool teacherPresent, bool teacherReachable, string id)
        {
            try
            {
                var result = await _connection.ExecuteAsync(
                    @"UPDATE narrowcast
                    SET eMail = @eMail,
                        phoneNumber = @phoneNumber,
                        teacherPresent = @teacherPresent,
                        teacherReachable = @teacherReachable
                    WHERE account_id in (
                        SELECT accountId
                        FROM accountids
                        WHERE accountType = 'teacher'
                        AND accountId = @id
                    );",
                    new {
                        eMail,
                        phoneNumber,
                        teacherPresent,
                        teacherReachable,
                        id
                    });

                return result;
            }
            catch (MySql.Data.MySqlClient.MySqlException eSql)
            {
                System.Diagnostics.Debug.WriteLine($"SQL Error: {eSql.Message} in {eSql.Source}");
                return 0;
            }
        }
    }
}