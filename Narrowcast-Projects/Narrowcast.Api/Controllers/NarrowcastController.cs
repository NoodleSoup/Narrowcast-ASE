using Narrowcast.Api.Domain;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Narrowcast.Api.Models;
using Newtonsoft.Json.Linq;

namespace Narrowcast.Api.Controllers
{
    /// <summary>
    /// Class to setup the API endpoint
    /// </summary>
    [ApiController, Route("narrowcast/v1.0")]
    public class NarrowcastController : ControllerBase
    {
        private readonly INarrowcastReadRepository _narrowcastRead;

        /// <summary>
        /// Constructor, cast narrowcastRead interface to readonly narrowcastRead.
        /// </summary>
        /// <param name="narrowcastRead"></param>
        public NarrowcastController(INarrowcastReadRepository narrowcastRead)
            => (_narrowcastRead) = (narrowcastRead);

        /// <summary>
        /// Endpoint to return all data or all data found by the specific search results
        /// </summary>
        /// <param name="teacherFirst"></param>
        /// <param name="courseName"></param>
        /// <param name="teacherLast"></param>
        /// <param name="teacherReachable"></param>
        /// <param name="updateDate"></param>
        /// <returns>
        /// Teacher data in JSON
        /// </returns>
        [HttpGet("search")]
        public async Task<IActionResult> CustomSearchNarrowcasts([FromQuery(Name = "teacherFirst")]string teacherFirst = "%",
            [FromQuery(Name = "courseName")]string courseName = "%", [FromQuery(Name = "teacherLast")]string teacherLast = "%",
            [FromQuery(Name = "teacherReachable")]string teacherReachable = "%", [FromQuery(Name = "updateDate")]string updateDate = "%")
        {
            if (updateDate == "current")
                updateDate = DateTime.Now.ToString("yyyy-MM-dd");
            else if (updateDate.Contains('/'))
                updateDate.Replace('/', '-');

            System.Diagnostics.Debug.WriteLine("Got these parameters: {0} {1} {2} {3}", teacherFirst, teacherLast, courseName, updateDate); // logging
            var result = await _narrowcastRead.CustomNarrowcastSearch(teacherFirst, teacherLast, courseName, teacherReachable, updateDate);
            return Ok(result);
        }

        /// <summary>
        /// Endpoint to get all courses
        /// </summary>
        /// <returns>
        /// All courses once in JSON
        /// </returns>
        [HttpGet("courses")]
        public async Task<IActionResult> GetCoursesAsync()
        {
            var result = await _narrowcastRead.GetCourses();
            return Ok(result);
        }

        /// <summary>
        /// Endpoint to get the teacher info by specific course
        /// </summary>
        /// <param name="course"></param>
        /// <returns>
        /// Teacher info per service
        /// </returns>
        [HttpGet("courses/list")]
        public async Task<IActionResult> GetNarrowcastByCourse([FromQuery(Name = "course")]string course = "%")
        {
            var result = await _narrowcastRead.GetNarrowcastByCourse(course);
            return Ok(result);
        }

        /// <summary>
        /// Endpoint to add account ID to the Database
        /// </summary>
        /// <param name="id"></param>
        /// <returns>
        /// int of affected rows
        /// </returns>
        [HttpGet("account/type")]
        public async Task<IActionResult> GetAccountType([FromQuery] string id)
        {
            var result = await _narrowcastRead.GetAccountType(id);
            return Ok(result);
        }

        /// <summary>
        /// Endpoint to add account ID to the Database
        /// </summary>
        /// <param name="account"></param>
        /// <returns>
        /// int of affected rows
        /// </returns>
        [HttpPost("account/id")]
        public async Task<IActionResult> AddId([FromBody]Account account)
        {
            var result = await _narrowcastRead.AddIdToDb(account.data.id, account.data.type);
            return Ok(result);
        }

        /// <summary>
        /// Endpoint to add account ID to the Database
        /// </summary>
        /// <param name="id"></param>
        /// <returns>
        /// int of affected rows
        /// </returns>
        [HttpGet("account/data")]
        public async Task<IActionResult> GetAccountData([FromQuery] string id)
        {
            var result = await _narrowcastRead.GetAccountData(id);
            return Ok(result);
        }

        /// <summary>
        /// Endpoint to add account ID to the Database
        /// </summary>
        /// <param name="accountData"></param>
        /// <returns>
        /// int of affected rows
        /// </returns>
        [HttpPost("account/data")]
        public async Task<IActionResult> SetAccountData([FromBody] AccountDataPost accountData)
        {
            var result = await _narrowcastRead.SetAccountData(accountData.data.eMail,
                accountData.data.phoneNumber,
                accountData.data.teacherPresent,
                accountData.data.teacherReachable,
                accountData.data.accountId);
            return Ok(result);
        }
    }
}