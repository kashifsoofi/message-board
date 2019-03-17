using System;
using System.Collections.Generic;
using System.Linq;
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

        [Fact]
        public void Should_return_user_messages()
        {
            var userId = _fixture.Create<string>();

            var messages = new List<MessageModel>
            {
                new MessageModel(Guid.NewGuid().ToString(), userId, _fixture.Create<string>(), DateTime.UtcNow),
                new MessageModel(Guid.NewGuid().ToString(), userId, _fixture.Create<string>(), DateTime.UtcNow),
                new MessageModel(Guid.NewGuid().ToString(), _fixture.Create<string>(), _fixture.Create<string>(), DateTime.UtcNow),
                new MessageModel(Guid.NewGuid().ToString(), _fixture.Create<string>(), _fixture.Create<string>(), DateTime.UtcNow),
            };
            _messages.AddRange(messages);

            var result = _sut.GetAll(userId);

            var messagesList = result?.ToList();
            messagesList.ShouldNotBeNull();
            messagesList.Count.ShouldBe(2);
        }

        [Fact]
        public void Should_return_if_user_does_not_exist()
        {
            _messages.AddRange(_fixture.Build<MessageModel>().CreateMany(5));

            var userId = Guid.NewGuid().ToString();

            var result = _sut.GetAll(userId);

            var messagesList = result?.ToList();
            messagesList.ShouldNotBeNull();
            messagesList.Count.ShouldBe(0);
        }
    }
}
