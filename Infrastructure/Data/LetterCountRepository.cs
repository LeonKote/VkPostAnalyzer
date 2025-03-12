using Domain.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
	public class LetterCountRepository : ILetterCountRepository
	{
		private readonly AppDbContext dbContext;

		public LetterCountRepository(AppDbContext dbContext)
		{
			this.dbContext = dbContext;
		}

		public async Task ClearAndAddRangeAsync(IEnumerable<LetterCount> letterCounts)
		{
			await dbContext.LetterCounts.ExecuteDeleteAsync();
			await dbContext.LetterCounts.AddRangeAsync(letterCounts);
			await dbContext.SaveChangesAsync();
		}
	}
}
