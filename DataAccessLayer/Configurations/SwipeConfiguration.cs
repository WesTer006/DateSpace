using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccessLayer.Configurations
{
	public class SwipeConfiguration : IEntityTypeConfiguration<Swipe>
	{
		public void Configure(EntityTypeBuilder<Swipe> builder)
		{
			builder.ToTable("Swipes");

			builder.Property(s => s.TargetAgree)
				   .HasDefaultValue(null);

			builder.Property(s => s.CreatedAt)
				   .HasDefaultValueSql("CURRENT_TIMESTAMP");

			builder.HasKey(s => new { s.SwiperId, s.TargetId });

			builder.HasOne(s => s.Swiper)
				   .WithMany(u => u.SwipesAsSwiper)
				   .HasForeignKey(s => s.SwiperId)
				   .OnDelete(DeleteBehavior.Cascade);

			builder.HasOne(s => s.Target)
			       .WithMany(u => u.SwipesAsTarget)
			       .HasForeignKey(s => s.TargetId)
			       .OnDelete(DeleteBehavior.NoAction);
		}
	}
}
