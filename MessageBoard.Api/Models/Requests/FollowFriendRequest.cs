using System;
namespace MessageBoard.Api.Models.Requests
{
    public class FollowFriendRequest
    {
        public string UserId { get; set; }
        public string FriendId { get; set; }
    }
}
