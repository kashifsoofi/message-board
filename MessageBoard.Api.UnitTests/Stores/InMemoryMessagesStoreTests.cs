using System.Collections.Generic;
using AutoFixture;
using MessageBoard.Api.Models;
using MessageBoard.Api.Stores;
using Shouldly;
using Xunit;

namespace MessageBoard.Api.UnitTests.Stores
{
    public class InMemoryMessagesStoreTests
    {
        private readonly IMessagesStore _sut;

        private readonly List<MessageModel> _messages;
        private readonly Fixture _fixture;

        public InMemoryMessagesStoreTests()
        {
            _messages = new List<MessageModel>();
            _sut = new InMemoryMessagesStore(_messages);
            _fixture = new Fixture();
        }

        [Fact]
        public void Should_store_message_model()
        {
            var messageModel = _fixture.Build<MessageModel>().Create();

            _sut.Store(messageModel);

            _messages.ShouldContain(messageModel);
        }
    }
}
