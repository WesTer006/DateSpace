using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DataAccessLayer.Entities;

namespace DataAccessLayer.Configurations
{
	public class PreferenceConfiguration : IEntityTypeConfiguration<Preference>
	{
		public void Configure(EntityTypeBuilder<Preference> builder)
		{
			builder.ToTable("Preferences");

			builder.HasKey(p => p.Id);

			builder.Property(p => p.PreferredGender)
				   .HasMaxLength(10)
				   .IsRequired()
				   .HasConversion<string>();

			builder.Property(p => p.MinAge)
				   .IsRequired();

			builder.Property(p => p.MaxAge)
				   .IsRequired();

			builder.Property(p => p.MaxDistance)
				   .IsRequired();

			builder.HasOne(p => p.User)
				   .WithOne(u => u.Preference)
				   .HasForeignKey<Preference>(p => p.UserId)
				   .OnDelete(DeleteBehavior.Cascade);
		}
	}
}
