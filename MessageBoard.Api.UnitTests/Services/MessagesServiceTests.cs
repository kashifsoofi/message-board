using AutoFixture;
using MessageBoard.Api.Models;
using MessageBoard.Api.Models.Requests;
using MessageBoard.Api.Services;
using MessageBoard.Api.Stores;
using Moq;
using Shouldly;
using Xunit;

namespace MessageBoard.Api.UnitTests.Services
{
    public class MessagesServiceTests
    {
        private readonly Fixture _fixture;
        private readonly Mock<IMessagesStore> _mockMessagesStore;

        private readonly IMessagesService _sut;

        public MessagesServiceTests()
        {
            _fixture = new Fixture();
            _mockMessagesStore = new Mock<IMessagesStore>();
            _sut = new MessagesService(_mockMessagesStore.Object);
        }

        [Fact]
        public void Should_store_and_return_message_model_when_creating()
        {
            var request = _fixture.Build<CreateMessageRequest>().Create();
            _mockMessagesStore.Setup(x => x.Store(It.Is<MessageModel>(m => m.UserId == request.UserId && m.Message == request.Message))).Verifiable();

            var createdMessage = _sut.Create(request);

            createdMessage.ShouldNotBeNull();
            createdMessage.UserId.ShouldBe(request.UserId);
            createdMessage.Message.ShouldBe(request.Message);

            _mockMessagesStore.Verify(x => x.Store(It.Is<MessageModel>(m => m.UserId == request.UserId && m.Message == request.Message)), Times.Once);
        }

        [Fact]
        public void Should_store_and_return_friends_model_when_following()
        {
            var request = _fixture.Build<FollowFriendRequest>().Create();

            _mockMessagesStore.Setup(x => x.Store(It.IsAny<FriendsModel>())).Verifiable();

            var createdFriend = _sut.Follow(request);

            createdFriend.ShouldNotBeNull();
            createdFriend.UserId.ShouldBe(request.UserId);
            createdFriend.FriendId.ShouldBe(request.FriendId);
        }

        [Fact]
        public void Should_not_store_friends_model_when_following_if_already_following()
        {
            var request = _fixture.Build<FollowFriendRequest>().Create();

            _mockMessagesStore.Setup(x => x.Store(It.IsAny<FriendsModel>())).Verifiable();

            var createdFriend = _sut.Follow(request);
            createdFriend.ShouldNotBeNull();
            createdFriend.UserId.ShouldBe(request.UserId);
            createdFriend.FriendId.ShouldBe(request.FriendId);

            var createdFriend2 = _sut.Follow(request);

            createdFriend2.ShouldBeNull();
        }
    }
}
