using Domain.Models;

namespace Domain.Interfaces.Repositories
{
	public interface ILetterCountRepository
	{
		Task ClearAndAddRangeAsync(IEnumerable<LetterCount> letterCounts);
	}
}
