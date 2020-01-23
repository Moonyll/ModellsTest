namespace Modells.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class picturesModells : DbContext
    {
        public picturesModells()
            : base("name=picturesModells")
        {
        }

        public virtual DbSet<admin> admin { get; set; }
        public virtual DbSet<category> category { get; set; }
        public virtual DbSet<comments> comments { get; set; }
        public virtual DbSet<picture> picture { get; set; }
        public virtual DbSet<users> users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<admin>()
                .Property(e => e.adminLogin)
                .IsUnicode(false);

            modelBuilder.Entity<admin>()
                .Property(e => e.adminPassword)
                .IsUnicode(false);

            modelBuilder.Entity<admin>()
                .Property(e => e.adminEmail)
                .IsUnicode(false);

            modelBuilder.Entity<admin>()
                .Property(e => e.adminPictureUrl)
                .IsUnicode(false);

            modelBuilder.Entity<category>()
                .Property(e => e.categoryName)
                .IsUnicode(false);

            modelBuilder.Entity<category>()
                .HasMany(e => e.picture)
                .WithRequired(e => e.category)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<comments>()
                .Property(e => e.commentContent)
                .IsUnicode(false);

            modelBuilder.Entity<comments>()
                .HasMany(e => e.users)
                .WithMany(e => e.comments)
                .Map(m => m.ToTable("post").MapLeftKey("commentId").MapRightKey("userId"));

            modelBuilder.Entity<picture>()
                .Property(e => e.pictureTitle)
                .IsUnicode(false);

            modelBuilder.Entity<picture>()
                .Property(e => e.pictureAlternateTitle)
                .IsUnicode(false);

            modelBuilder.Entity<picture>()
                .Property(e => e.pictureDescription)
                .IsUnicode(false);

            modelBuilder.Entity<picture>()
                .Property(e => e.pictureStandardUrl)
                .IsUnicode(false);

            modelBuilder.Entity<picture>()
                .HasMany(e => e.comments)
                .WithRequired(e => e.picture)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<users>()
                .Property(e => e.userLogin)
                .IsUnicode(false);

            modelBuilder.Entity<users>()
                .Property(e => e.userPassword)
                .IsUnicode(false);

            modelBuilder.Entity<users>()
                .Property(e => e.userEmail)
                .IsUnicode(false);

            modelBuilder.Entity<users>()
                .Property(e => e.userPictureUrl)
                .IsUnicode(false);
        }
    }
}
