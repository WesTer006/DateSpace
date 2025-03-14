using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DataAccessLayer.Entities;

namespace DataAccessLayer.Configurations
{
	public class MessageConfiguration : IEntityTypeConfiguration<Message>
	{
		public void Configure(EntityTypeBuilder<Message> builder)
		{
			builder.ToTable("Messages");

			builder.Property(m => m.MessageText)
				   .IsRequired()
				   .HasColumnType("TEXT");

			builder.Property(m => m.CreatedAt)
				   .HasDefaultValueSql("CURRENT_TIMESTAMP");

			builder.HasOne(m => m.Sender)
				   .WithMany(u => u.SentMessages)
				   .HasForeignKey(m => m.SenderId)
				   .OnDelete(DeleteBehavior.NoAction);

			builder.HasOne(m => m.Receiver)
				   .WithMany(u => u.ReceivedMessages)
				   .HasForeignKey(m => m.ReceiverId)
				   .OnDelete(DeleteBehavior.NoAction);
		}
	}
}
