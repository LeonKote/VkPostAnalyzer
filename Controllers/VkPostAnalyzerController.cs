using Microsoft.AspNetCore.Mvc;
using VkPostAnalyzer.Domain.DTO;
using VkPostAnalyzer.Domain.Interfaces;

namespace VkPostAnalyzer.Controllers
{
	[ApiController]
	[Route("api/vk/posts")]
	public class VkPostAnalyzerController : ControllerBase
	{
		private readonly IVkPostAnalyzerService vkPostAnalyzerService;

		public VkPostAnalyzerController(IVkPostAnalyzerService vkPostAnalyzerService)
		{
			this.vkPostAnalyzerService = vkPostAnalyzerService;
		}

		/// <summary>
		/// Анализирует последние 5 постов указанного пользователя VK и подсчитывает частоту букв.
		/// </summary>
		/// <param name="analyzeRequest">Запрос, содержащий токен доступа VK и идентификатор владельца постов (ownerId необязателен).</param>
		/// <returns>Список с результатами анализа (количество вхождений каждой буквы).</returns>
		[HttpPost("analyze")]
		public async Task<IActionResult> AnalyzePosts([FromBody] AnalyzeRequest analyzeRequest)
		{
			var result = await vkPostAnalyzerService.AnalyzePosts(analyzeRequest.AccessToken!, analyzeRequest.OwnerId); 
			if (result.IsSuccess)
				return Ok(new { result = result.Value });
			return BadRequest(new { error = result.Error });
		}
	}
}
