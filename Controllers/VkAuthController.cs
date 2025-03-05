using Microsoft.AspNetCore.Mvc;
using VkPostAnalyzer.Domain.Interfaces;

namespace VkPostAnalyzer.Controllers
{
	[ApiController]
	[Route("api/vk/auth")]
	public class VkAuthController : ControllerBase
	{
		private readonly IVkAuthService vkAuthService;

		public VkAuthController(IVkAuthService vkAuthService)
		{
			this.vkAuthService = vkAuthService;
		}

		/// <summary>
		/// Генерация URL для авторизации в VK (OAuth 2.0).
		/// </summary>
		/// <returns>URL для авторизации.</returns>
		[HttpGet("url")]
		public async Task<IActionResult> GetAuthUrl()
		{
			var result = await vkAuthService.GenerateAuthUrl();
			if (result.IsSuccess)
				return Ok(new { url = result.Value });
			return BadRequest(new { error = result.Error });
		}

		/// <summary>
		/// Обработка ответа после авторизации VK (OAuth 2.0).
		/// </summary>
		/// <param name="code">Код авторизации.</param>
		/// <param name="deviceId">ID устройства.</param>
		/// <param name="state">Состояние для валидации запроса.</param>
		/// <returns>Токен доступа VK.</returns>
		[HttpGet("response")]
		public async Task<IActionResult> HandleAuthResponse([FromQuery] string code, [FromQuery(Name = "device_id")] string deviceId, [FromQuery] string state)
		{
			var result = await vkAuthService.HandleAuthResponse(code, deviceId, state);
			if (result.IsSuccess)
				return Ok(new { accessToken = result.Value });
			return BadRequest(new { error = result.Error });
		}
	}
}
