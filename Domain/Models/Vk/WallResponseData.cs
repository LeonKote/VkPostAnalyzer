using System.Text.Json.Serialization;

namespace Domain.Models.Vk
{
	public class WallResponseData
	{
		[JsonPropertyName("items")]
		public WallPost[]? Items { get; set; }
	}
}
