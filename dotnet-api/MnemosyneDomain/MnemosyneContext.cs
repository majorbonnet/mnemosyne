using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using MnemosyneDomain.Models;

namespace MnemosyneDomain;

public partial class MnemosyneContext : DbContext
{
    public MnemosyneContext(DbContextOptions<MnemosyneContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Image> Images { get; set; }

    public virtual DbSet<Journal> Journals { get; set; }

    public virtual DbSet<JournalPage> JournalPages { get; set; }

    public virtual DbSet<UserInfo> UserInfos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Image>(entity =>
        {
            entity.HasKey(e => e.ImageId).HasName("image_pkey");

            entity.ToTable("image");

            entity.Property(e => e.ImageId)
                .ValueGeneratedNever()
                .HasColumnName("image_id");
            entity.Property(e => e.AltText).HasColumnName("alt_text");
            entity.Property(e => e.Created).HasColumnName("created");
            entity.Property(e => e.ImageKey)
                .HasMaxLength(20)
                .HasColumnName("image_key");
            entity.Property(e => e.Updated).HasColumnName("updated");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.Images)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_image_user_info");
        });

        modelBuilder.Entity<Journal>(entity =>
        {
            entity.HasKey(e => e.JournalId).HasName("journal_pkey");

            entity.ToTable("journal");

            entity.Property(e => e.JournalId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("journal_id");
            entity.Property(e => e.Created).HasColumnName("created");
            entity.Property(e => e.Title)
                .HasMaxLength(200)
                .HasColumnName("title");
            entity.Property(e => e.Updated).HasColumnName("updated");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.Journals)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_journal_user_info");
        });

        modelBuilder.Entity<JournalPage>(entity =>
        {
            entity.HasKey(e => e.JournalPageId).HasName("journal_page_pkey");

            entity.ToTable("journal_page");

            entity.Property(e => e.JournalPageId)
                .ValueGeneratedNever()
                .HasColumnName("journal_page_id");
            entity.Property(e => e.Contents).HasColumnName("contents");
            entity.Property(e => e.Created).HasColumnName("created");
            entity.Property(e => e.JournalId).HasColumnName("journal_id");
            entity.Property(e => e.PageNumber).HasColumnName("page_number");
            entity.Property(e => e.Title)
                .HasMaxLength(200)
                .HasColumnName("title");
            entity.Property(e => e.Updated).HasColumnName("updated");

            entity.HasOne(d => d.Journal).WithMany(p => p.JournalPages)
                .HasForeignKey(d => d.JournalId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_journal_page_journal");
        });

        modelBuilder.Entity<UserInfo>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("user_info_pkey");

            entity.ToTable("user_info");

            entity.Property(e => e.UserId)
                .ValueGeneratedNever()
                .HasColumnName("user_id");
            entity.Property(e => e.DisplayName)
                .HasMaxLength(100)
                .HasColumnName("display_name");
            entity.Property(e => e.LastLogin).HasColumnName("last_login");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
