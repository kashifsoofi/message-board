using MessageBoard.Api.Models;

namespace MessageBoard.Api.Stores
{
    public interface IMessagesStore
    {
        void Store(MessageModel messageModel);
    }
}
