using System.Collections.Generic;
using MessageBoard.Api.Models;

namespace MessageBoard.Api.Stores
{
    public interface IMessagesStore
    {
        void Store(MessageModel messageModel);
        IEnumerable<MessageModel> GetAll(string userId);
    }
}
