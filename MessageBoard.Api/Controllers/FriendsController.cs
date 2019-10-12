using MessageBoard.Api.Models.Requests;
using MessageBoard.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace MessageBoard.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class FriendsController : ControllerBase
    {
        private readonly IMessagesService _messagesService;

        public FriendsController(IMessagesService messagesService)
        {
            _messagesService = messagesService;
        }

        [HttpPost]
        public IActionResult Post(FollowFriendRequest request)
        {
            try
            {
                var messageModel = _messagesService.Follow(request);
                return Ok(messageModel);
            }
            catch
            {
                return StatusCode(500);
            }
        }
    }
}
