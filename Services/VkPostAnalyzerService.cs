using VkPostAnalyzer.Domain.Errors;
using VkPostAnalyzer.Domain.Interfaces;
using VkPostAnalyzer.Domain.Models;
using VkPostAnalyzer.Infrastructure.Data;

namespace VkPostAnalyzer.Services
{
	public class VkPostAnalyzerService : IVkPostAnalyzerService
	{
		private readonly AppDbContext dbContext;
		private readonly IVkApiClient vkApiClient;
		private readonly ILogger<VkPostAnalyzerService> logger;

		public VkPostAnalyzerService(AppDbContext dbContext, IVkApiClient vkApiClient, ILogger<VkPostAnalyzerService> logger)
		{
			this.dbContext = dbContext;
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

			await dbContext.LetterCounts.AddRangeAsync(entities);
			await dbContext.SaveChangesAsync();
			logger.LogInformation("Данные успешно сохранены.");

			return Result<List<LetterCount>>.Success(entities);
		}

		private static char GetAlphabeticalOrder(char c) => c == 'ё' ? 'е' : c;
	}
}
