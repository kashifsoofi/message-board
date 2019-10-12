using System;
using AutoFixture;
using MessageBoard.Api.Controllers;
using MessageBoard.Api.Models;
using MessageBoard.Api.Models.Requests;
using MessageBoard.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Shouldly;
using Xunit;

namespace MessageBoard.Api.UnitTests
{
    public class FriendsControllerTests
    {
        private readonly Fixture _fixture;
        private readonly Mock<IMessagesService> _mockMessagesService;

        private readonly FriendsController _sut;

        public FriendsControllerTests()
        {
            _fixture = new Fixture();
            _mockMessagesService = new Mock<IMessagesService>();
            _sut = new FriendsController(_mockMessagesService.Object);
        }

        [Fact]
        public void Should_return_ok_and_message_when_post_successful()
        {
            var request = _fixture.Build<FollowFriendRequest>().Create();
            var message = new FriendsModel(request.UserId, request.FriendId, DateTime.UtcNow);
            _mockMessagesService
                .Setup(x => x.Follow(It.Is<FollowFriendRequest>(m => m.UserId == request.UserId && m.FriendId == request.FriendId)))
                .Returns(message);

            var result = _sut.Post(request);

            var okResult = result as OkObjectResult;
            okResult.ShouldNotBeNull();
            okResult.StatusCode?.ShouldBe(200);

            var createdMessage = okResult.Value as FriendsModel;
            createdMessage.ShouldNotBeNull();
            createdMessage.ShouldBe(message);

            _mockMessagesService.Verify(x => x.Follow(It.Is<FollowFriendRequest>(m => m.UserId == request.UserId && m.FriendId == request.FriendId)), Times.Once);
        }

        [Fact]
        public void Should_return_internal_server_error_when_post_unsuccessful()
        {
            var request = _fixture.Build<FollowFriendRequest>().Create();
            _mockMessagesService
                .Setup(x => x.Follow(It.Is<FollowFriendRequest>(m => m.UserId == request.UserId && m.FriendId == request.FriendId)))
                .Throws(new Exception());

            var result = _sut.Post(request);

            var statusCodeResult = result as StatusCodeResult;
            statusCodeResult.StatusCode.ShouldBe(500);
        }
    }
}
