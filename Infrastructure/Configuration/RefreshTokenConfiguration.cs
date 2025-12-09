using Domain.Entities;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.HasKey(x => x.Id);
        builder.ToTable("RefreshTokens");

        // Token properties
        builder.Property(x => x.Token)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(x => x.CreatedAt)
            .IsRequired()
            .HasColumnType("timestamp with time zone");

        builder.Property(x => x.CreatedByIp)
            .IsRequired()
            .HasMaxLength(45); // IPv6 max length

        builder.Property(x => x.RevokedAt)
            .HasColumnType("timestamp with time zone")
            .IsRequired(false);

        builder.Property(x => x.RevokedByIp)
            .HasMaxLength(45)
            .IsRequired(false);

        builder.Property(x => x.ReplacedByToken)
            .HasMaxLength(256)
            .IsRequired(false);

        builder.Property(x => x.Expires)
            .IsRequired()
            .HasColumnType("timestamp with time zone");

        // Foreign key relationship
        builder.HasOne<ApplicationUser>()
            .WithMany(u => u.RefreshTokens)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Indexes for better query performance
        builder.HasIndex(x => x.Token).IsUnique();
        builder.HasIndex(x => x.UserId);
    }
}