using ChatNet.Domain.Entity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace ChatNet.Domain.Configuration;

public class MessageConfiguration : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        builder.ToTable("Messages");
        builder.HasKey(x => x.Id);
        builder.Property(m => m.TextMessage).IsRequired();

        builder.HasOne(u => u.Sender)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(u => u.Reciver)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);
    }
}
