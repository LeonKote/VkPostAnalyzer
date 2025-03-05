using System.Text.Json.Serialization;

namespace VkPostAnalyzer.Domain.Models.Vk
{
	public class WallResponse
	{
		[JsonPropertyName("response")]
		public WallResponseData? Response { get; set; }
	}
}
