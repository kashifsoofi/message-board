using System.Collections.Generic;
using System.Linq;
using MessageBoard.Api.Models;

namespace MessageBoard.Api.Stores
{
    public class InMemoryMessagesStore : IMessagesStore
    {
        private readonly List<MessageModel> _repository;

        public InMemoryMessagesStore(List<MessageModel> repository)
        {
            _repository = repository;
        }

        public InMemoryMessagesStore()
            : this(new List<MessageModel>())
        { }

        public void Store(MessageModel messageModel)
        {
            _repository.Add(messageModel);
        }

        public IEnumerable<MessageModel> GetAll(string userId)
        {
            return _repository.Where(x => x.UserId == userId);
        }
    }
}
