using System;
namespace MessageBoard.Api.Models
{
    public class MessageModel
    {
        public string MessageId { get; }
        public string UserId { get; }
        public string Message { get; }
        public DateTime CreatedDate { get; }

        public MessageModel(string messageId, string userId, string message, DateTime createdDate)
        {
            MessageId = messageId;
            UserId = userId;
            Message = message;
            CreatedDate = createdDate;
        }
    }
}
