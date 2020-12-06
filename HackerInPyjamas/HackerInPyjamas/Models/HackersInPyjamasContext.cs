using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace HackerInPyjamas.Models
{
    public partial class HackersInPyjamasContext : DbContext
    {
        public HackersInPyjamasContext()
        {
        }

        public HackersInPyjamasContext(DbContextOptions<HackersInPyjamasContext> options)
            : base(options)
        {
        }

        public virtual DbSet<IndexedLink> IndexedLinks { get; set; }
        public virtual DbSet<ReportedPost> ReportedPosts { get; set; }
        public virtual DbSet<SpecificAdress> SpecificAdresses { get; set; }
        public virtual DbSet<UserReport> UserReports { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=DESKTOP-F4JJ5HS;Database=HackersInPyjamas;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IndexedLink>(entity =>
            {
                entity.ToTable("indexedLinks");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Date)
                    .HasColumnType("date")
                    .HasColumnName("date");

                entity.Property(e => e.Link).HasColumnName("link");

                entity.Property(e => e.PageIndex).HasColumnName("pageIndex");

                entity.Property(e => e.Text).HasColumnName("text");
            });

            modelBuilder.Entity<ReportedPost>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("title");
            });

            modelBuilder.Entity<SpecificAdress>(entity =>
            {
                entity.ToTable("specificAdresses");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Class)
                    .HasMaxLength(50)
                    .HasColumnName("class");

                entity.Property(e => e.Path)
                    .IsUnicode(false)
                    .HasColumnName("path");

                entity.Property(e => e.SearchClass)
                    .HasMaxLength(50)
                    .HasColumnName("searchClass");

                entity.Property(e => e.Website)
                    .IsUnicode(false)
                    .HasColumnName("website");
            });

            modelBuilder.Entity<UserReport>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ReportedPostId).HasColumnName("ReportedPostID");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .IsUnicode(false)
                    .HasColumnName("userID");

                entity.HasOne(d => d.ReportedPost)
                    .WithMany(p => p.UserReports)
                    .HasForeignKey(d => d.ReportedPostId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Report_Users");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
