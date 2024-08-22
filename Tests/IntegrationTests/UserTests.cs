using Application.DTO.Application.User;
using Core.Response;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using System.Text;

namespace Tests.IntegrationTests
{
    public class UserTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> factory;

        public UserTests(WebApplicationFactory<Program> factory)
        {
            this.factory = factory;
        }

        [Theory]
        [InlineData("Password85*", UseCaseResponseKind.Ok)]
        [InlineData("password", UseCaseResponseKind.BadRequest)]
        public async Task TestRegisterAdmin(string password, UseCaseResponseKind expectedStatus)
        {
            HttpClient client = this.factory.CreateClient();

            RegisterAdminRequestDTO requestDTO = new()
            {
                UserName = Guid.NewGuid().ToString().AsSpan(0, 10).ToString(),
                Password = password
            };

            StringContent content = new StringContent(JsonConvert.SerializeObject(requestDTO), Encoding.UTF8, "application/json");

            HttpResponseMessage httpResponseMessage = await client.PostAsync($"/api/v1/user/register-admin", content);
            String responseText = await httpResponseMessage.Content.ReadAsStringAsync();

            UseCaseResponse<RegisterAdminResponseDTO> actualResponse = JsonConvert.DeserializeObject<UseCaseResponse<RegisterAdminResponseDTO>>(responseText);
            UseCaseResponse<RegisterAdminResponseDTO> expectedResponse = new()
            {
                Status = expectedStatus
            };

            Assert.Equal(expectedResponse.Status, actualResponse.Status);
        }
        
    }
}
