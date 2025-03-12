using Domain.Interfaces.Repositories;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories
{
	public class AuthRequestRepository : IAuthRequestRepository
	{
		private readonly AppDbContext dbContext;

		public AuthRequestRepository(AppDbContext dbContext)
		{
			this.dbContext = dbContext;
		}

		public async Task AddAsync(AuthRequest authRequest)
		{
			await dbContext.AuthRequests.AddAsync(authRequest);
			await dbContext.SaveChangesAsync();
		}

		public async Task<AuthRequest?> GetByStateAsync(string state)
		{
			return await dbContext.AuthRequests.FirstOrDefaultAsync(x => x.State == state);
		}

		public async Task RemoveAsync(AuthRequest authRequest)
		{
			dbContext.AuthRequests.Remove(authRequest);
			await dbContext.SaveChangesAsync();
		}
	}
}
