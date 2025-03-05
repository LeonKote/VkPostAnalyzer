using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using VkPostAnalyzer.Domain.Errors;
using VkPostAnalyzer.Domain.Interfaces;
using VkPostAnalyzer.Domain.Models;
using VkPostAnalyzer.Infrastructure.Data;

namespace VkPostAnalyzer.Services
{
	public class VkAuthService : IVkAuthService
	{
		private readonly AppDbContext dbContext;
		private readonly IVkApiClient vkApiClient;
		private readonly ILogger<VkPostAnalyzerService> logger;

		public VkAuthService(AppDbContext dbContext, IVkApiClient vkApiClient, ILogger<VkPostAnalyzerService> logger)
		{
			this.dbContext = dbContext;
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
			await dbContext.AuthRequests.AddAsync(authRequest);
			await dbContext.SaveChangesAsync();

			return Result<string>.Success(authUrl);
		}

		public async Task<Result<string>> HandleAuthResponse(string code, string deviceId, string state)
		{
			logger.LogInformation("Обработка ответа VK");

			var authRequest = await dbContext.AuthRequests.FirstOrDefaultAsync(x => x.State == state);
			if (authRequest == null)
				return Result<string>.Failure("Invalid state parameter.");

			dbContext.AuthRequests.Remove(authRequest);
			await dbContext.SaveChangesAsync();

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
