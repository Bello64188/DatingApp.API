using DatingApp.API.Data;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Configuration.Entities
{
    public class LikeConfiguration : IEntityTypeConfiguration<Like>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Like> builder)
        {
            builder.HasKey(k=>new {k.likeeId,k.likerId});

            builder.HasOne(u=>u.likee)
            .WithMany(u=>u.Likers)
            .HasForeignKey(k=>k.likerId)
            .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(u=>u.liker)
            .WithMany(u=>u.Likees)
            .HasForeignKey(k=>k.likeeId)
            .OnDelete(DeleteBehavior.Restrict);
        }
    }
}