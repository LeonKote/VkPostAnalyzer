using System.Text.Json.Serialization;

namespace Domain.Models.Vk
{
	public class GetAccessTokenResponse
	{
		[JsonPropertyName("access_token")]
		public string? AccessToken { get; set; }
	}
}
