using System;
using Microsoft.EntityFrameworkCore;
using Test.DataModels;
namespace Test.DataLayer
{
	public class RaghuDbContext : DbContext
	{
		public RaghuDbContext(DbContextOptions<RaghuDbContext> options) : base(options)
		{
		}

		public DbSet<User> Users { get; set; }
		public DbSet<Favourite> Favourites { get; set; }

		public DbSet<PendingRegistration> PendingRegistration { get; set; }

		public DbSet<Country> Countries { get; set; }



		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<PendingRegistration>()
				.ToTable("PendingRegistration", "Global");
			modelBuilder.Entity<Country>()
				.ToTable("Countries", "Global");
		}

	}
}

