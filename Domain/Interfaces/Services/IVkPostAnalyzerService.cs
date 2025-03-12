using Domain.Errors;
using Domain.Models;

namespace Domain.Interfaces.Services
{
	public interface IVkPostAnalyzerService
	{
		Task<Result<List<LetterCount>>> AnalyzePosts(string accessToken, long ownerId);
	}
}
