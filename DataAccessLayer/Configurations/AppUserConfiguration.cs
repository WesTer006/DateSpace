using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccessLayer.Configurations
{
	public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
	{
		public void Configure(EntityTypeBuilder<AppUser> builder)
		{
			builder.ToTable("AppUser", t =>
			{
				t.HasCheckConstraint("CK_AppUser_Age", "[Age] >= 18");
			});

			builder.Property(u => u.Age).IsRequired();
			builder.Property(u => u.Email).IsRequired();
			builder.Property(u => u.Gender).HasMaxLength(10).IsRequired();
			builder.Property(u => u.Bio).HasColumnType("NVARCHAR(MAX)");
			builder.Property(u => u.CreatedAt).HasDefaultValueSql("GETDATE()");

			builder.Property(u => u.RefreshToken)
	               .HasMaxLength(150)  
	               .IsRequired(false); 

			builder.Property(u => u.RefreshTokenExpiryTime)
				   .IsRequired(false);  

			builder.HasMany(u => u.Photos)
				   .WithOne()
				   .HasForeignKey(p => p.UserId)
				   .OnDelete(DeleteBehavior.Cascade);

			builder.HasOne(u => u.Preference)
				   .WithOne()
				   .HasForeignKey<Preference>(p => p.UserId)
				   .OnDelete(DeleteBehavior.Cascade);

			builder.HasMany(u => u.SwipesAsSwiper)
				   .WithOne(s => s.Swiper)
				   .HasForeignKey(s => s.SwiperId)
				   .OnDelete(DeleteBehavior.Cascade);

			builder.HasMany(u => u.SwipesAsTarget)
				   .WithOne(s => s.Target)
				   .HasForeignKey(s => s.TargetId)
				   .OnDelete(DeleteBehavior.NoAction);

			builder.HasMany(u => u.SentMessages)
				   .WithOne()
				   .HasForeignKey(m => m.SenderId)
				   .OnDelete(DeleteBehavior.NoAction);

			builder.HasMany(u => u.ReceivedMessages)
				   .WithOne()
				   .HasForeignKey(m => m.ReceiverId)
				   .OnDelete(DeleteBehavior.NoAction);

			builder.HasOne(u => u.Location)
				   .WithOne()
				   .HasForeignKey<Location>(l => l.UserId)
				   .OnDelete(DeleteBehavior.Cascade);
		}
	}
}
