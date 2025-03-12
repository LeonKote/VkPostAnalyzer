using System.Text.Json.Serialization;

namespace Domain.Models.Vk
{
	public class WallPost
	{
		[JsonPropertyName("text")]
		public string? Text { get; set; }
	}
}
