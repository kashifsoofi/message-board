namespace MessageBoard.Api.Models.Requests
{
    public class CreateMessageRequest
    {
        public string UserId { get; set; }
        public string Message { get; set; }
    }
}
