
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using StudentAPIBusinessLayer.Interface;

namespace Stuednt.Test
{
    public class StudentApiFactory : WebApplicationFactory<Program>
    {
        public const string TestJwtSecret =
            "TestSecretKey_123456789012345678901234567890";

        public Mock<IStudentService> StudentServiceMock { get; } = new();

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Development");

            builder.ConfigureAppConfiguration((context, config) =>
            {
                var settings = new Dictionary<string, string?>
                {
                    ["JWT_SECRET_KEY"] = TestJwtSecret
                };

                config.AddInMemoryCollection(settings);
            });

            builder.ConfigureServices(services =>
            {
                services.RemoveAll<IStudentService>();

                services.AddScoped<IStudentService>(
                    _ => StudentServiceMock.Object);
            });
        }
    }
}
