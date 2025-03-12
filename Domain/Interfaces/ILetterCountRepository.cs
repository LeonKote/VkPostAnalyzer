using Domain.Models;

namespace Domain.Interfaces
{
	public interface ILetterCountRepository
	{
		Task AddRangeAsync(IEnumerable<LetterCount> letterCounts);
	}
}
