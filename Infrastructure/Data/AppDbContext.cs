using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
	public class AppDbContext : DbContext
	{
		public DbSet<AuthRequest> AuthRequests { get; set; }
		public DbSet<LetterCount> LetterCounts { get; set; }

		public AppDbContext(DbContextOptions options) : base(options)
		{
			Database.EnsureCreated();
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<LetterCount>(entity =>
			{
				entity.HasKey(e => e.Letter);
				entity.Property(e => e.Letter).ValueGeneratedNever();
			});
		}
	}
}
