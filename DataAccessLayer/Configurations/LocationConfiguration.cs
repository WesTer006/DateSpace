using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DataAccessLayer.Entities;

namespace DataAccessLayer.Configurations
{
	public class LocationConfiguration : IEntityTypeConfiguration<Location>
	{
		public void Configure(EntityTypeBuilder<Location> builder)
		{
			builder.ToTable("Locations");

			builder.Property(l => l.Latitude)
				   .IsRequired();

			builder.Property(l => l.Longitude)
				   .IsRequired();

			builder.Property(l => l.GeoLocation)
				   .HasColumnType("geography") // SQL Server требует явного указания
				   .IsRequired();

			builder.Property(l => l.UpdatedAt)
				   .HasDefaultValueSql("CURRENT_TIMESTAMP");

			builder.HasOne(l => l.User)
				   .WithOne(u => u.Location)
				   .HasForeignKey<Location>(l => l.UserId)
				   .OnDelete(DeleteBehavior.Cascade);
		}
	}
}
