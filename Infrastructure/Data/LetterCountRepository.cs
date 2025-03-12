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
			// Провайдер базы данных для тестов не поддерживает этот метод
			// await dbContext.LetterCounts.ExecuteDeleteAsync();
			var allEntries = await dbContext.LetterCounts.ToListAsync();
			dbContext.LetterCounts.RemoveRange(allEntries);
			await dbContext.LetterCounts.AddRangeAsync(letterCounts);
			await dbContext.SaveChangesAsync();
		}
	}
}
