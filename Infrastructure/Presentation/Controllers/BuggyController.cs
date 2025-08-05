using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [ApiController]
    [Route(template:"api/[controller]")]
    public class BuggyController : ControllerBase
    {
        [HttpGet(template:"notfound")] //GET: /api/Buggy/notfound
        public IActionResult GetNotFoundRequest()
        {
            //Code
            return NotFound(); //404
        }

        [HttpGet(template: "servererror")] //GET: /api/Buggy/servererror
        public IActionResult GetServerErrorRequest()
        {
            //Code
            throw new Exception();
            return Ok(); //404
        }

        [HttpGet(template: "badrequest")] //GET: /api/Buggy/badrequest
        public IActionResult GetBadRequest()
        {
            //Code
            return BadRequest(); //404
        }

        [HttpGet(template: "badrequest/{id}")] //GET: /api/Buggy/badrequest
        public IActionResult GetBadRequest(int id) //Validation Error
        {
            //Code
            return BadRequest(); //404
        }

        [HttpGet(template: "unauthorized")] //GET: /api/Buggy/unauthorized
        public IActionResult GetUnauthorizedRequest()
        {
            //Code
            return Unauthorized(); //401
        }

    }
}
