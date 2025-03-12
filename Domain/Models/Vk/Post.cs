using System.Text.Json.Serialization;

namespace Domain.Models.Vk
{
	public class Post
	{
		[JsonPropertyName("text")]
		public string? Text { get; set; }
	}
}
