using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using AutoFixture;
using MessageBoard.Api.Models;
using MessageBoard.Api.Stores;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

namespace MessageBoard.Api.AcceptanceTests
{
    public class TestServerFixture : IDisposable
    {
        private readonly TestServer _testServer;
        public HttpClient Client { get; }
        public List<MessageModel> Messages { get; }

        public TestServerFixture()
        {
            var fixture = new Fixture();
            Messages = fixture.Build<MessageModel>().CreateMany(10).ToList();

            var builder = new WebHostBuilder()
                .UseSolutionRelativeContentRoot("MessageBoard.Api")
                .UseEnvironment("Development")
                .ConfigureServices(services =>
                {
                    services.AddSingleton<IMessagesStore, InMemoryMessagesStore>(_ => new InMemoryMessagesStore(Messages));
                })
                .UseStartup<Startup>();

            _testServer = new TestServer(builder);
            Client = _testServer.CreateClient();
        }

        public void Dispose()
        {
            Client.Dispose();
            _testServer.Dispose();
        }
    }
}
