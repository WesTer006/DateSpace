using DataAccessLayer.Configurations;
using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Data
{
	public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<AppUser>(options)
	{
		public required DbSet<Photo> Photos { get; set; }
		public required DbSet<Preference> Preferences { get; set; }
		public required DbSet<Swipe> Swipes { get; set; }
		public required DbSet<Location> Locations { get; set; }
		public required DbSet<Message> Messages { get; set; }

		protected override void  OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			builder.ApplyConfiguration(new AppUserConfiguration());
			builder.ApplyConfiguration(new PhotoConfiguration());
			builder.ApplyConfiguration(new PreferenceConfiguration());
			builder.ApplyConfiguration(new SwipeConfiguration());
			builder.ApplyConfiguration(new LocationConfiguration());
			builder.ApplyConfiguration(new MessageConfiguration());
		}
	}
}
