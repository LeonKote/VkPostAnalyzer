using VkPostAnalyzer.Domain.Errors;
using VkPostAnalyzer.Domain.Models;

namespace VkPostAnalyzer.Domain.Interfaces
{
	public interface IVkPostAnalyzerService
	{
		Task<Result<List<LetterCount>>> AnalyzePosts(string accessToken, long ownerId);
	}
}
