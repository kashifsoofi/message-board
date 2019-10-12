using System;
namespace MessageBoard.Api.Models
{
    public class FriendsModel
    {
        public string UserId { get; }
        public string FriendId { get; }
        public DateTime CreatedDate { get; }

        public FriendsModel(string userId, string friendId, DateTime createdDate)
        {
            UserId = userId;
            FriendId = friendId;
            CreatedDate = createdDate;
        }
    }
}
