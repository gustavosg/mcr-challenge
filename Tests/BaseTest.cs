using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.AspNetCore;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace Tests
{
    public class BaseTest
    {
        public TestServer CreateServer()
        {
            Assembly.Load("Api");
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var webHostBuilder = WebHost.CreateDefaultBuilder()
                .UseConfiguration(config)
                .UseSetting("https_port", "7275");

            webHostBuilder.UseStartup<Startup>();

            TestServer testServer = new TestServer(webHostBuilder)
            {
                BaseAddress = new System.Uri("https://localhost:7275/"),

            };
            return testServer;
        }
    }
}
