using System.Collections.Generic;
using MessageBoard.Api.Models.Requests;
using MessageBoard.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace MessageBoard.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly IMessagesService _messagesService;

        public MessagesController(IMessagesService messagesService)
        {
            _messagesService = messagesService;
        }

        [HttpPost]
        public IActionResult Post(CreateMessageRequest request)
        {
            try
            {
                var messageModel = _messagesService.Create(request);
                return Ok(messageModel);
            }
            catch
            {
                return StatusCode(500);
            }
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new List<string> { "value1", "value2" });
        }
    }
}
