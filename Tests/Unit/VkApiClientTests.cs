using Domain.Models.Vk;
using Infrastructure.ApiClients;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using System.Net;
using System.Text;
using System.Text.Json;
using Xunit;

namespace Tests.Unit
{
	public class VkApiClientTests
	{
		private readonly Mock<HttpMessageHandler> httpMessageHandlerMock;
		private readonly HttpClient httpClient;
		private readonly IOptions<VkApiOptions> vkOptions;
		private readonly VkApiClient vkApiClient;

		public VkApiClientTests()
		{
			httpMessageHandlerMock = new Mock<HttpMessageHandler>();
			httpClient = new HttpClient(httpMessageHandlerMock.Object);
			vkOptions = Options.Create(new VkApiOptions
			{
				ClientId = "test_client_id",
				RedirectUri = "https://test_redirect_uri",
				Version = "5.131"
			});
			vkApiClient = new VkApiClient(httpClient, vkOptions);
		}

		[Fact]
		public void GetAuthUrl_ShouldReturnCorrectUrl()
		{
			// Arrange
			var state = "test_state";
			var codeChallenge = "test_code_challenge";

			// Act
			var result = vkApiClient.GetAuthUrl(state, codeChallenge);

			// Assert
			Assert.Contains($"https://id.vk.com/authorize?response_type=code&client_id={vkOptions.Value.ClientId}&code_challenge={codeChallenge}&code_challenge_method=S256&redirect_uri={vkOptions.Value.RedirectUri}&state={state}", result);
		}

		[Fact]
		public async Task GetAccessToken_ShouldReturnToken()
		{
			// Arrange
			var expectedToken = "test_access_token";
			var jsonResponse = JsonSerializer.Serialize(new { access_token = expectedToken });

			httpMessageHandlerMock.Protected()
				.Setup<Task<HttpResponseMessage>>("SendAsync",
					ItExpr.IsAny<HttpRequestMessage>(),
					ItExpr.IsAny<CancellationToken>())
				.ReturnsAsync(new HttpResponseMessage
				{
					StatusCode = HttpStatusCode.OK,
					Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json")
				});

			// Act
			var result = await vkApiClient.GetAccessToken("code", "device_id", "code_verifier");

			// Assert
			Assert.Equal(expectedToken, result);
		}

		[Fact]
		public async Task GetPosts_ShouldReturnWallResponseData()
		{
			// Arrange
			var expectedResponse = new WallResponse { Response = new WallResponseData { Items = new[] { new WallPost { Text = "Test post" } } } };
			var jsonResponse = JsonSerializer.Serialize(expectedResponse);

			httpMessageHandlerMock.Protected()
				.Setup<Task<HttpResponseMessage>>("SendAsync",
					ItExpr.IsAny<HttpRequestMessage>(),
					ItExpr.IsAny<CancellationToken>())
				.ReturnsAsync(new HttpResponseMessage
				{
					StatusCode = HttpStatusCode.OK,
					Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json")
				});

			// Act
			var result = await vkApiClient.GetPosts("access_token", 5, 0);

			// Assert
			Assert.NotNull(result);
			Assert.Single(result.Items);
			Assert.Equal("Test post", result.Items[0].Text);
		}
	}
}
