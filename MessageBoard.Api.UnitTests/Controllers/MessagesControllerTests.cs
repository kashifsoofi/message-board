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
    public class MessagesControllerTests
    {
        private readonly Fixture _fixture;
        private readonly Mock<IMessagesService> _mockMessagesService;

        private readonly MessagesController _sut;

        public MessagesControllerTests()
        {
            _fixture = new Fixture();
            _mockMessagesService = new Mock<IMessagesService>();
            _sut = new MessagesController(_mockMessagesService.Object);
        }

        [Fact]
        public void Should_return_ok_and_message_when_post_successful()
        {
            var request = _fixture.Build<CreateMessageRequest>().Create();
            var message = new MessageModel(Guid.NewGuid().ToString(), request.UserId, request.Message, DateTime.UtcNow);
            _mockMessagesService
                .Setup(x => x.Create(It.Is<CreateMessageRequest>(m => m.UserId == request.UserId && m.Message == request.Message)))
                .Returns(message);

            var result = _sut.Post(request);

            var okResult = result as OkObjectResult;
            okResult.ShouldNotBeNull();
            okResult.StatusCode?.ShouldBe(200);

            var createdMessage = okResult.Value as MessageModel;
            createdMessage.ShouldNotBeNull();
            createdMessage.ShouldBe(message);

            _mockMessagesService.Verify(x => x.Create(It.Is<CreateMessageRequest>(m => m.UserId == request.UserId && m.Message == request.Message)), Times.Once);
        }

        [Fact]
        public void Should_return_internal_server_error_when_post_unsuccessful()
        {
            var request = _fixture.Build<CreateMessageRequest>().Create();
            _mockMessagesService
                .Setup(x => x.Create(It.Is<CreateMessageRequest>(m => m.UserId == request.UserId && m.Message == request.Message)))
                .Throws(new Exception());

            var result = _sut.Post(request);

            var statusCodeResult = result as StatusCodeResult;
            statusCodeResult.StatusCode.ShouldBe(500);
        }
    }
}
