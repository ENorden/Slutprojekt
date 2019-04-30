using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Slutprojekt.Models.Entities
{
    public partial class SlutprojektContext : DbContext
    {
        public SlutprojektContext()
        {
        }

        public SlutprojektContext(DbContextOptions<SlutprojektContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<Follower> Follower { get; set; }
        public virtual DbSet<Ingredient> Ingredient { get; set; }
        public virtual DbSet<Recipe> Recipe { get; set; }
        public virtual DbSet<Recipe2Category> Recipe2Category { get; set; }
        public virtual DbSet<SavedRecipe> SavedRecipe { get; set; }
        public virtual DbSet<StepByStep> StepByStep { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=SlutprojektDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.3-servicing-35854");

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Category", "rec");

                entity.Property(e => e.CategoryName).IsRequired();
            });

            modelBuilder.Entity<Follower>(entity =>
            {
                entity.ToTable("Follower", "rec");

                entity.Property(e => e.FollowerId)
                    .IsRequired()
                    .HasColumnName("FollowerID")
                    .HasMaxLength(450);

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasColumnName("UserID")
                    .HasMaxLength(450);
            });

            modelBuilder.Entity<Ingredient>(entity =>
            {
                entity.ToTable("Ingredient", "rec");

                entity.Property(e => e.Name).IsRequired();

                entity.Property(e => e.RecId).HasColumnName("RecID");

                entity.Property(e => e.Unit)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Rec)
                    .WithMany(p => p.Ingredient)
                    .HasForeignKey(d => d.RecId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Ingredien__RecID__00200768");
            });

            modelBuilder.Entity<Recipe>(entity =>
            {
                entity.ToTable("Recipe", "rec");

                entity.Property(e => e.Img).IsRequired();

                entity.Property(e => e.Title).IsRequired();

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasColumnName("UserID")
                    .HasMaxLength(450);
            });

            modelBuilder.Entity<Recipe2Category>(entity =>
            {
                entity.ToTable("Recipe2Category", "rec");

                entity.Property(e => e.CatId).HasColumnName("CatID");

                entity.Property(e => e.RecId).HasColumnName("RecID");

                entity.HasOne(d => d.Cat)
                    .WithMany(p => p.Recipe2Category)
                    .HasForeignKey(d => d.CatId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Recipe2Ca__CatID__02084FDA");

                entity.HasOne(d => d.Rec)
                    .WithMany(p => p.Recipe2Category)
                    .HasForeignKey(d => d.RecId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Recipe2Ca__RecID__01142BA1");
            });

            modelBuilder.Entity<SavedRecipe>(entity =>
            {
                entity.ToTable("SavedRecipe", "rec");

                entity.Property(e => e.RecId).HasColumnName("RecID");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasColumnName("UserID")
                    .HasMaxLength(450);

                entity.HasOne(d => d.Rec)
                    .WithMany(p => p.SavedRecipe)
                    .HasForeignKey(d => d.RecId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SavedReci__RecID__02FC7413");
            });

            modelBuilder.Entity<StepByStep>(entity =>
            {
                entity.ToTable("StepByStep", "rec");

                entity.Property(e => e.Instruction).IsRequired();

                entity.Property(e => e.RecId).HasColumnName("RecID");

                entity.HasOne(d => d.Rec)
                    .WithMany(p => p.StepByStep)
                    .HasForeignKey(d => d.RecId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__StepBySte__RecID__04E4BC85");
            });
        }
    }
}
