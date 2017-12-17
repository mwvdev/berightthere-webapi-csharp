using System;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;

namespace BeRightThereTests.IntegrationTests
{
    public class TestFixture<TStartup> : IDisposable
    {
        private readonly TestServer _server;

        public TestFixture()
        {
            var builder = new WebHostBuilder()
                .UseEnvironment("Development")
                .UseKestrel()
                .UseIISIntegration()
                .UseStartup(typeof(TStartup))
                .UseApplicationInsights();
            _server = new TestServer(builder);

            Client = _server.CreateClient();
            Client.BaseAddress = new Uri("http://localhost:50649");
        }

        public HttpClient Client { get; }

        public void Dispose()
        {
            Client.Dispose();
            _server.Dispose();
        }
    }
}