using Domain.Interfaces;
using Domain.Models;

namespace Infrastructure.Data
{
	public class LetterCountRepository : ILetterCountRepository
	{
		private readonly AppDbContext dbContext;

		public LetterCountRepository(AppDbContext dbContext)
		{
			this.dbContext = dbContext;
		}

		public async Task AddRangeAsync(IEnumerable<LetterCount> letterCounts)
		{
			await dbContext.LetterCounts.AddRangeAsync(letterCounts);
			await dbContext.SaveChangesAsync();
		}
	}
}
