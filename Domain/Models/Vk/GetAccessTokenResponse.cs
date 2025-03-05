using System.Text.Json.Serialization;

namespace VkPostAnalyzer.Domain.Models.Vk
{
	public class GetAccessTokenResponse
	{
		[JsonPropertyName("access_token")]
		public string? AccessToken { get; set; }
	}
}
