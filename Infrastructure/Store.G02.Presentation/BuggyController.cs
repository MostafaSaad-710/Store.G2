using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.G02.Presentation
{
    [ApiController]
    [Route("api/[controller]")]
    public class BuggyController : ControllerBase
    {
        [HttpGet("notfound")] // GET: /api/Buggy/notfound
        public IActionResult GetNotFoundRequest()
        {
            // Code
            return NotFound(); // 404
        }

        [HttpGet("servererror")] // GET: /api/Buggy/servererror
        public IActionResult GetServerError()
        {
            throw new Exception();
            return Ok();
        }

        [HttpGet(template:"badrequest")] // GET: /api/Buggy/badrequest
        public IActionResult GetBadRequest()
        {
            // Code
            return BadRequest(); // 400
        }

        [HttpGet(template:"badrequest/{id}/{age}")] // GET: /api/Buggy/badrequest/ahmed
        public IActionResult GetBadRequest(int id, int age) // ValidationError
        {
            // Code
            return BadRequest(); // 400
        }

        [HttpGet(template: "unauthorized")] // GET: /api/Buggy/unauthorized
        public IActionResult GetUnauthorizedRequest()
        {
            // Code
            return Unauthorized(); // 401
        }

    }
}
