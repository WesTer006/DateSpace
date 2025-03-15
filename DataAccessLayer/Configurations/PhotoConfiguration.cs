using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DataAccessLayer.Entities;

namespace DataAccessLayer.Configurations
{
	public class PhotoConfiguration : IEntityTypeConfiguration<Photo>
	{
		public void Configure(EntityTypeBuilder<Photo> builder)
		{
			builder.ToTable("Photos");

			builder.Property(p => p.Url)
				   .IsRequired()
				   .HasColumnType("NVARCHAR(MAX)");

			builder.Property(p => p.IsMain)
				   .HasDefaultValue(false);

			builder.HasOne(p => p.User)
				   .WithMany(u => u.Photos)
				   .HasForeignKey(p => p.UserId)
				   .OnDelete(DeleteBehavior.Cascade);
		}
	}
}
