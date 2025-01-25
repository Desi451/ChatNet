using ChatNet.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatNet.Domain.Configuration;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Name).IsRequired();
        builder.Property(t => t.Email).IsRequired();
        builder.Property(t => t.Password).IsRequired();

        builder.HasMany(u => u.Friends)
               .WithMany()
               .UsingEntity<Dictionary<string, object>>(
                   "UserFriends",
                   j => j.HasOne<User>().WithMany().HasForeignKey("FriendId"),
                   j => j.HasOne<User>().WithMany().HasForeignKey("UserId")
               );

        builder.HasMany(u => u.InvitedFriends)
               .WithMany()
               .UsingEntity<Dictionary<string, object>>(
                   "UserInvitedFriends",
                   j => j.HasOne<User>().WithMany().HasForeignKey("InvitedFriendId"),
                   j => j.HasOne<User>().WithMany().HasForeignKey("UserId")
               );
    }
}
