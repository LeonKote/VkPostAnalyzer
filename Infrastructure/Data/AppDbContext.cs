using Microsoft.EntityFrameworkCore;
using VkPostAnalyzer.Domain.Models;

namespace VkPostAnalyzer.Infrastructure.Data
{
	public class AppDbContext : DbContext
	{
		public DbSet<AuthRequest> AuthRequests { get; set; }
		public DbSet<LetterCount> LetterCounts { get; set; }

		public AppDbContext(DbContextOptions options) : base(options)
		{
			Database.EnsureCreated();
		}
	}
}
