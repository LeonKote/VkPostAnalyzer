using Domain.Models;

namespace Domain.Interfaces
{
	public interface ILetterCountRepository
	{
		Task ClearAndAddRangeAsync(IEnumerable<LetterCount> letterCounts);
	}
}
