using Domain.Interfaces;
using Domain.Models;
using Microsoft.Extensions.Logging;
using Moq;
using Services;
using Xunit;

namespace Tests.Unit
{
	public class VkAuthServiceTests
	{
		private readonly Mock<IAuthRequestRepository> authRequestRepositoryMock;
		private readonly Mock<IVkApiClient> vkApiClientMock;
		private readonly Mock<ILogger<VkAuthService>> loggerMock;
		private readonly VkAuthService vkAuthService;

		public VkAuthServiceTests()
		{
			authRequestRepositoryMock = new Mock<IAuthRequestRepository>();
			vkApiClientMock = new Mock<IVkApiClient>();
			loggerMock = new Mock<ILogger<VkAuthService>>();
			vkAuthService = new VkAuthService(authRequestRepositoryMock.Object, vkApiClientMock.Object, loggerMock.Object);
		}

		[Fact]
		public async Task GenerateAuthUrl_ShouldReturnUrl()
		{
			// Arrange
			var expectedUrl = "https://id.vk.com/authorize?someparams";

			vkApiClientMock.Setup(client => client.GetAuthUrl(It.IsAny<string>(), It.IsAny<string>()))
				.Returns(expectedUrl);
			authRequestRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<AuthRequest>()))
				.Returns(Task.CompletedTask);
			
			// Act
			var result = await vkAuthService.GenerateAuthUrl();

			// Assert
			Assert.True(result.IsSuccess);
			Assert.Equal(expectedUrl, result.Value);
		}

		[Fact]
		public async Task HandleAuthResponse_ShouldReturnAccessToken_WhenValid()
		{
			// Arrange
			var code = "auth_code";
			var deviceId = "device_123";
			var state = "valid_state";
			var expectedToken = "access_token";
			var authRequest = new AuthRequest(state, "verifier_code", System.DateTime.UtcNow);

			authRequestRepositoryMock.Setup(repo => repo.GetByStateAsync(state))
				.ReturnsAsync(authRequest);
			authRequestRepositoryMock.Setup(repo => repo.RemoveAsync(authRequest))
				.Returns(Task.CompletedTask);
			vkApiClientMock.Setup(client => client.GetAccessToken(code, deviceId, authRequest.CodeVerifier))
				.ReturnsAsync(expectedToken);

			// Act
			var result = await vkAuthService.HandleAuthResponse(code, deviceId, state);

			// Assert
			Assert.True(result.IsSuccess);
			Assert.Equal(expectedToken, result.Value);
		}

		[Fact]
		public async Task HandleAuthResponse_ShouldReturnFailure_WhenStateInvalid()
		{
			// Arrange
			var code = "auth_code";
			var deviceId = "device_123";
			var state = "invalid_state";

			authRequestRepositoryMock.Setup(repo => repo.GetByStateAsync(state))
				.ReturnsAsync((AuthRequest?)null);

			// Act
			var result = await vkAuthService.HandleAuthResponse(code, deviceId, state);

			// Assert
			Assert.False(result.IsSuccess);
			Assert.Equal("Invalid state parameter.", result.Error);
		}
	}
}
