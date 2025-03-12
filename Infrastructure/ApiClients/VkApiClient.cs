using Domain.Interfaces;
using Domain.Models.Vk;
using Microsoft.Extensions.Options;
using System.Text;
using System.Text.Json;

namespace Infrastructure.ApiClients
{
	public class VkApiClient : IVkApiClient
	{
		private readonly HttpClient httpClient;
		private readonly string clientId;
		private readonly string redirectUri;
		private readonly string version;

		public VkApiClient(HttpClient httpClient, IOptions<VkApiOptions> options)
		{
			this.httpClient = httpClient;

			var vkApiOptions = options.Value;

			clientId = vkApiOptions.ClientId ?? throw new InvalidOperationException("Vk ClientId is not configured.");
			redirectUri = vkApiOptions.RedirectUri ?? throw new InvalidOperationException("Vk RedirectUri is not configured.");
			version = vkApiOptions.Version ?? throw new InvalidOperationException("Vk Version is not configured.");
		}

		public string GetAuthUrl(string state, string codeChallenge)
		{
			return $"https://id.vk.com/authorize?response_type=code&client_id={clientId}&code_challenge={codeChallenge}&code_challenge_method=S256&redirect_uri={redirectUri}&state={state}";
		}

		public async Task<string?> GetAccessToken(string code, string deviceId, string codeVerifier)
		{
			var response = await PostAsync("https://id.vk.com/oauth2/auth",
				$"grant_type=authorization_code&code={code}&code_verifier={codeVerifier}&client_id={clientId}&device_id={deviceId}&redirect_uri={redirectUri}");
			return JsonSerializer.Deserialize<GetAccessTokenResponse>(response)?.AccessToken;
		}

		public async Task<WallResponseData?> GetPosts(string accessToken, int count, long ownerId = 0)
		{
			var response = await PostAsync("https://api.vk.com/method/wall.get",
				$"owner_id={(ownerId == 0 ? "" : ownerId)}&count={count}&access_token={accessToken}&v={version}");
			return JsonSerializer.Deserialize<WallResponse>(response)?.Response;
		}

		private async Task<string> PostAsync(string url, string args)
		{
			using var content = new StringContent(args, Encoding.UTF8, "application/x-www-form-urlencoded");
			using var response = await httpClient.PostAsync(url, content);
			return await response.Content.ReadAsStringAsync();
		}
	}
}
