using DataAccessLayer.Configurations;
using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Data
{
	public class AppDbContext:IdentityDbContext<AppUser>
	{
		public DbSet<Photo> Photos { get; set; }
		public DbSet<Preference> Preferences { get; set; }
		public DbSet<Swipe> Swipes { get; set; }
		public DbSet<Location> Locations { get; set; }
		public DbSet<Message> Messages { get; set; }

		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
		{
			
		}
		// TODO read about S927
		protected override void  OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.ApplyConfiguration(new AppUserConfiguration());
			modelBuilder.ApplyConfiguration(new PhotoConfiguration());
			modelBuilder.ApplyConfiguration(new PreferenceConfiguration());
			modelBuilder.ApplyConfiguration(new SwipeConfiguration());
			modelBuilder.ApplyConfiguration(new LocationConfiguration());
			modelBuilder.ApplyConfiguration(new MessageConfiguration());
		}
	}
}
