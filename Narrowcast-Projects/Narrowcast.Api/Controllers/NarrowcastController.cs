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
    [ApiController, Route("narrowcast")]
    public class NarrowcastController : ControllerBase
    {
        private readonly INarrowcastReadRepository _narrowcastRead;

        /// <summary>
        /// Constructor, cast containerRead interface to readonly containerRead.
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
        /// Image/Container data in JSON
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
        /// Endpoint to get all services
        /// </summary>
        /// <returns>
        /// All services once in JSON
        /// </returns>
        [HttpGet("courses")]
        public async Task<IActionResult> GetCoursesAsync()
        {
            var result = await _narrowcastRead.GetCourses();
            return Ok(result);
        }

        /// <summary>
        /// Endpoint to get the image/container info by specific service
        /// </summary>
        /// <param name="course"></param>
        /// <returns>
        /// Image/Container info per service
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
        /// <param name="account"></param>
        /// <returns>
        /// int of affected rows
        /// </returns>
        [HttpPost("account/id")]
        public async Task<IActionResult> AddId([FromBody]Account account)
        {
            // For debugging
            Console.WriteLine($"\n\n\n\nGot ID: {account.data.id} Type: {account.data.type}\n\n\n\n");
            var result = await _narrowcastRead.AddIdToDb(account.data.id, account.data.type);
            return Ok(result);
        }
    }
}