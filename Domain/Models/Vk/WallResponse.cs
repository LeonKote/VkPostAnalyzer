using System.Text.Json.Serialization;

namespace Domain.Models.Vk
{
	public class WallResponse
	{
		[JsonPropertyName("response")]
		public WallResponseData? Response { get; set; }
	}
}
