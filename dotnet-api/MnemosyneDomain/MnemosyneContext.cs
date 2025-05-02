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

    public virtual DbSet<Notebook> Notebooks { get; set; }

    public virtual DbSet<Page> Pages { get; set; }

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

        modelBuilder.Entity<Notebook>(entity =>
        {
            entity.HasKey(e => e.NotebookId).HasName("notebook_pkey");

            entity.ToTable("notebook");

            entity.Property(e => e.NotebookId)
                .ValueGeneratedNever()
                .HasColumnName("notebook_id");
            entity.Property(e => e.Created).HasColumnName("created");
            entity.Property(e => e.Title)
                .HasMaxLength(200)
                .HasColumnName("title");
            entity.Property(e => e.Updated).HasColumnName("updated");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.Notebooks)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_notebook_user_info");
        });

        modelBuilder.Entity<Page>(entity =>
        {
            entity.HasKey(e => e.PageId).HasName("page_pkey");

            entity.ToTable("page");

            entity.Property(e => e.PageId)
                .ValueGeneratedNever()
                .HasColumnName("page_id");
            entity.Property(e => e.Contents).HasColumnName("contents");
            entity.Property(e => e.Created).HasColumnName("created");
            entity.Property(e => e.NotebookId).HasColumnName("notebook_id");
            entity.Property(e => e.PageNumber).HasColumnName("page_number");
            entity.Property(e => e.Title)
                .HasMaxLength(200)
                .HasColumnName("title");
            entity.Property(e => e.Updated).HasColumnName("updated");

            entity.HasOne(d => d.Notebook).WithMany(p => p.Pages)
                .HasForeignKey(d => d.NotebookId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_page_notebook");
        });

        modelBuilder.Entity<UserInfo>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("user_info_pkey");

            entity.ToTable("user_info");

            entity.Property(e => e.UserId)
                .ValueGeneratedNever()
                .HasColumnName("user_id");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
