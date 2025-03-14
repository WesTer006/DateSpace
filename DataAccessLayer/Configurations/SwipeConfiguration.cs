using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;


namespace DataAccessLayer.Configurations
{
	public class SwipeConfiguration : IEntityTypeConfiguration<Swipe>
	{
		public void Configure(EntityTypeBuilder<Swipe> builder)
		{
			builder.ToTable("Swipes");

			builder.Property(s => s.TargetAgree)
				   .HasDefaultValue(null); // NULL означает, что второй пользователь не ответил

			builder.Property(s => s.CreatedAt)
				   .HasDefaultValueSql("CURRENT_TIMESTAMP");

			builder.HasKey(s => new { s.SwiperId, s.TargetId });

			builder.HasOne(s => s.Swiper)
				   .WithMany(u => u.Swipes)
				   .HasForeignKey(s => s.SwiperId)
				   .OnDelete(DeleteBehavior.NoAction);

			builder.HasOne(s => s.Target)
				   .WithMany(u => u.Swipes)
				   .HasForeignKey(s => s.TargetId)
				   .OnDelete(DeleteBehavior.NoAction);
		}
	}
}
