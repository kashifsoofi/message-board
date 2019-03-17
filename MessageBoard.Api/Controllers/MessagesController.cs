using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace MessageBoard.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new List<string> { "value1", "value2" });
        }
    }
}
