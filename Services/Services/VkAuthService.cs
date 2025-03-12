using Domain.Errors;
using Domain.Interfaces.ApiClients;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using System.Text;

namespace Services
{
	public class VkAuthService : IVkAuthService
	{
		private readonly IAuthRequestRepository authRequestRepository;
		private readonly IVkApiClient vkApiClient;
		private readonly ILogger<VkAuthService> logger;

		public VkAuthService(IAuthRequestRepository authRequestRepository, IVkApiClient vkApiClient, ILogger<VkAuthService> logger)
		{
			this.authRequestRepository = authRequestRepository;
			this.vkApiClient = vkApiClient;
			this.logger = logger;
		}

		public async Task<Result<string>> GenerateAuthUrl()
		{
			logger.LogInformation("Генерация ссылки авторизации...");

			var codeVerifier = GenerateCodeVerifier();
			var codeChallenge = GetCodeChallenge(codeVerifier);
			var state = GenerateState();
			var authUrl = vkApiClient.GetAuthUrl(state, codeChallenge);

			var authRequest = new AuthRequest(state, codeVerifier, DateTime.UtcNow);
			await authRequestRepository.AddAsync(authRequest);

			return Result<string>.Success(authUrl);
		}

		public async Task<Result<string>> HandleAuthResponse(string code, string deviceId, string state)
		{
			logger.LogInformation("Обработка ответа VK");

			var authRequest = await authRequestRepository.GetByStateAsync(state);
			if (authRequest == null)
				return Result<string>.Failure("Invalid state parameter.");

			await authRequestRepository.RemoveAsync(authRequest);

			var accessToken = await vkApiClient.GetAccessToken(code, deviceId, authRequest.CodeVerifier);
			if (accessToken == null)
				return Result<string>.Failure("Error getting access token.");

			return Result<string>.Success(accessToken);
		}

		private static string GenerateCodeVerifier() => Base64UrlTextEncoder.Encode(RandomNumberGenerator.GetBytes(16));
		private static string GenerateState() => Base64UrlTextEncoder.Encode(RandomNumberGenerator.GetBytes(8));
		private static string GetCodeChallenge(string codeVerifier) =>
			Base64UrlTextEncoder.Encode(SHA256.HashData(Encoding.UTF8.GetBytes(codeVerifier)));
	}
}
