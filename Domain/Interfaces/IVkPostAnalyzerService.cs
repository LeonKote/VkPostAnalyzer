using Domain.Errors;
using Domain.Models;

namespace Domain.Interfaces
{
	public interface IVkPostAnalyzerService
	{
		Task<Result<List<LetterCount>>> AnalyzePosts(string accessToken, long ownerId);
	}
}
