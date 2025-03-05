using System.Text.Json.Serialization;

namespace VkPostAnalyzer.Domain.Models.Vk
{
	public class Post
	{
		[JsonPropertyName("text")]
		public string? Text { get; set; }
	}
}
