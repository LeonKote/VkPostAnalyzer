using Domain.Models;
using Infrastructure.Data;
using Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Tests.Repositories
{
	public class AuthRequestRepositoryTests : IDisposable
	{
		private readonly AppDbContext dbContext;
		private readonly AuthRequestRepository repository;

		public AuthRequestRepositoryTests()
		{
			var options = new DbContextOptionsBuilder<AppDbContext>()
				.UseInMemoryDatabase(Guid.NewGuid().ToString())
				.Options;
			dbContext = new AppDbContext(options);
			repository = new AuthRequestRepository(dbContext);
		}

		[Fact]
		public async Task AddAsync_ShouldAddAuthRequest()
		{
			var authRequest = new AuthRequest("testState", "codeVerifier", DateTime.UtcNow);
			await repository.AddAsync(authRequest);

			var savedRequest = await dbContext.AuthRequests.FirstOrDefaultAsync(x => x.State == "testState");
			Assert.NotNull(savedRequest);
			Assert.Equal("codeVerifier", savedRequest.CodeVerifier);
		}

		[Fact]
		public async Task GetByStateAsync_ShouldReturnCorrectAuthRequest()
		{
			var authRequest = new AuthRequest("testState", "codeVerifier", DateTime.UtcNow);
			dbContext.AuthRequests.Add(authRequest);
			await dbContext.SaveChangesAsync();

			var retrievedRequest = await repository.GetByStateAsync("testState");
			Assert.NotNull(retrievedRequest);
			Assert.Equal("codeVerifier", retrievedRequest.CodeVerifier);
		}

		[Fact]
		public async Task RemoveAsync_ShouldDeleteAuthRequest()
		{
			var authRequest = new AuthRequest("testState", "codeVerifier", DateTime.UtcNow);
			dbContext.AuthRequests.Add(authRequest);
			await dbContext.SaveChangesAsync();

			await repository.RemoveAsync(authRequest);
			var deletedRequest = await dbContext.AuthRequests.FirstOrDefaultAsync(x => x.State == "testState");
			Assert.Null(deletedRequest);
		}

		public void Dispose()
		{
			dbContext.Dispose();
		}
	}
}
