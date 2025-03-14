using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Configurations
{
	public  class AppUserConfiguration:IEntityTypeConfiguration<AppUser>
	{
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.ToTable("AppUser");

			// Конфигурируем свойства
			builder.Property(u => u.Gender)
				   .HasMaxLength(10)
				   .IsRequired();

			builder.Property(u => u.Bio)
				   .HasColumnType("NVARCHAR(MAX)");

			builder.Property(u => u.CreatedAt)
				   .HasDefaultValueSql("GETDATE()");

			builder.HasMany(u => u.Photos)
				   .WithOne()
				   .HasForeignKey(p => p.UserId)
				   .OnDelete(DeleteBehavior.NoAction);

			builder.HasOne(u => u.Preference)
				   .WithOne()
				   .HasForeignKey<Preference>(p => p.UserId)
				   .OnDelete(DeleteBehavior.NoAction);

			builder.HasMany(u => u.Swipes)
				   .WithOne()
				   .HasForeignKey(s => s.SwiperId)
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