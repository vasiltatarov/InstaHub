namespace MyForum.Web.Tests
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc.Testing;

    using Xunit;

    public class WebTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> server;

        public WebTests(WebApplicationFactory<Startup> server)
        {
            this.server = server;
        }

        [Theory]
        [InlineData("/")]
        [InlineData("/Categories/GetCategories")]
        [InlineData("/Posts/Create")]
        [InlineData("/Chat")]
        public async Task IndexPageShouldReturnStatusCode200AndCorrectContent(string url)
        {
            var client = this.server.CreateClient();

            var response = await client.GetAsync(url);
            var responseContent = await response.Content.ReadAsStringAsync();

            Assert.True(response.IsSuccessStatusCode);
            Assert.Contains("utf-8", responseContent);
        }

        [Theory]
        [InlineData("/")]
        public async Task IndexPageShouldReturnCorrectContent(string url)
        {
            var client = this.server.CreateClient();

            var response = await client.GetAsync(url);
            var responseContent = await response.Content.ReadAsStringAsync();

            Assert.Contains("<i class=\"fas fa-home fa-fw\"></i>Home", responseContent);
        }

        [Theory]
        [InlineData("/notExist")]
        public async Task NonExistingPageShouldReturnNonSuccessStatusCode(string url)
        {
            var client = this.server.CreateClient();

            var response = await client.GetAsync(url);

            Assert.False(response.IsSuccessStatusCode);
        }
    }
}
