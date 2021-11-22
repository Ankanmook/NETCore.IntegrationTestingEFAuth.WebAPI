using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Xunit;

namespace NETCore.IntegrationTestingEFAuth.WebAPI.IntegrationTests.Controllers
{
    public class WeatherForecastControllerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;

        public WeatherForecastControllerTests(WebApplicationFactory<Startup> factory)
        {
            _client = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                        {
                            services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                        });
            }).CreateClient();
        }

        [Fact]
        public async Task Get()
        {
            var response = (await _client.GetAsync("/weatherforecast")).EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<WeatherForecast[]>(stringResponse);

            Assert.Equal(5, result.Length);
        }
    }
}