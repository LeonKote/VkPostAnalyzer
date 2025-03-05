using VkPostAnalyzer.Domain.Models.Vk;

namespace VkPostAnalyzer.Domain.Interfaces
{
	public interface IVkApiClient
	{
		string GetAuthUrl(string state, string codeChallenge);
		Task<string?> GetAccessToken(string code, string deviceId, string codeVerifier);
		Task<WallResponseData?> GetPosts(string accessToken, int count, long ownerId);
	}
}
