using Domain.Models;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Tests.Unit
{
	public class LetterCountRepositoryTests : IDisposable
	{
		private readonly AppDbContext dbContext;
		private readonly LetterCountRepository repository;

		public LetterCountRepositoryTests()
		{
			var options = new DbContextOptionsBuilder<AppDbContext>()
				.UseInMemoryDatabase(Guid.NewGuid().ToString())
				.Options;
			dbContext = new AppDbContext(options);
			repository = new LetterCountRepository(dbContext);
		}

		[Fact]
		public async Task ClearAndAddRangeAsync_ShouldReplaceLetterCounts()
		{
			var initialData = new List<LetterCount>
			{
				new LetterCount('a', 5), new LetterCount('b', 3)
			};
			dbContext.LetterCounts.AddRange(initialData);
			await dbContext.SaveChangesAsync();

			var newData = new List<LetterCount>
			{
				new LetterCount('c', 7), new LetterCount('d', 2)
			};
			await repository.ClearAndAddRangeAsync(newData);

			var storedData = await dbContext.LetterCounts.ToListAsync();
			Assert.Equal(2, storedData.Count);
			Assert.Contains(storedData, lc => lc.Letter == 'c' && lc.Count == 7);
			Assert.Contains(storedData, lc => lc.Letter == 'd' && lc.Count == 2);
		}

		public void Dispose()
		{
			dbContext.Dispose();
		}
	}
}
