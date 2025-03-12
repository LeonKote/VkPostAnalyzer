using Domain.Errors;
using Domain.Interfaces.ApiClients;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Models;
using Microsoft.Extensions.Logging;

namespace Services
{
	public class VkPostAnalyzerService : IVkPostAnalyzerService
	{
		private readonly ILetterCountRepository letterCountRepository;
		private readonly IVkApiClient vkApiClient;
		private readonly ILogger<VkPostAnalyzerService> logger;

		public VkPostAnalyzerService(ILetterCountRepository letterCountRepository, IVkApiClient vkApiClient, ILogger<VkPostAnalyzerService> logger)
		{
			this.letterCountRepository = letterCountRepository;
			this.vkApiClient = vkApiClient;
			this.logger = logger;
		}

		public async Task<Result<List<LetterCount>>> AnalyzePosts(string accessToken, long ownerId)
		{
			logger.LogInformation("Запрос постов пользователя VK...");
			var response = await vkApiClient.GetPosts(accessToken, 5, ownerId);
			if (response == null)
				return Result<List<LetterCount>>.Failure("Error requesting posts.");

			var posts = response.Items.Select(x => x.Text);
			var text = string.Join(' ', posts);

			logger.LogInformation("Посты получены, начинается анализ...");
			var letterCounts = text.ToLower()
				.Where(char.IsLetter)
				.GroupBy(c => c)
				.OrderBy(g => GetAlphabeticalOrder(g.Key));

			var entities = letterCounts.Select(g => new LetterCount(g.Key, g.Count())).ToList();
			logger.LogInformation("Анализ завершен, сохранение данных в БД...");

			await letterCountRepository.ClearAndAddRangeAsync(entities);
			logger.LogInformation("Данные успешно сохранены.");

			return Result<List<LetterCount>>.Success(entities);
		}

		private static char GetAlphabeticalOrder(char c) => c == 'ё' ? 'е' : c;
	}
}
