using System.Text.Json.Serialization;

namespace VkPostAnalyzer.Domain.Models.Vk
{
	public class WallResponseData
	{
		[JsonPropertyName("items")]
		public List<Post>? Items { get; set; }
	}
}
