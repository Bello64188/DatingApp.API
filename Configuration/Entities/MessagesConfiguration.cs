using DatingApp.API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DatingApp.API.Configuration.Entities
{

    public class MessagesConfiguration : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder.HasOne(u=>u.sender)
            .WithMany(u=>u.MessageSent)
            .HasForeignKey(u=>u.senderId)
            .OnDelete(DeleteBehavior.Restrict);

            
            builder.HasOne(u=>u.recipient)
            .WithMany(u=>u.MessageReceived)
            .HasForeignKey(u=>u.recipientId)
            .OnDelete(DeleteBehavior.Restrict);
        }
    }
}