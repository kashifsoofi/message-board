using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AutoFixture;
using MessageBoard.Api.Models;
using MessageBoard.Api.Models.Requests;
using Newtonsoft.Json;
using Shouldly;
using Xunit;

namespace MessageBoard.Api.AcceptanceTests
{
    public class MessagesControllerTests : IClassFixture<TestServerFixture>
    {
        private readonly TestServerFixture _testFixture;
        private readonly Fixture _fixture;

        public MessagesControllerTests(TestServerFixture testFixture)
        {
            _testFixture = testFixture;
            _fixture = new Fixture();
        }

        [Fact]
        public async Task Should_return_ok_and_message_model_on_post()
        {
            var request = _fixture.Build<CreateMessageRequest>().Create();

            var requestContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var response = await _testFixture.Client.PostAsync("api/messages", requestContent);

            response.StatusCode.ShouldBe(HttpStatusCode.OK);
            var responseContent = await response.Content.ReadAsStringAsync();

            var createdMessageModel = JsonConvert.DeserializeObject<MessageModel>(responseContent);
            createdMessageModel.UserId.ShouldBe(request.UserId);
            createdMessageModel.Message.ShouldBe(request.Message);
        }
    }
}
