using Microsoft.EntityFrameworkCore;
using Goal_Achievement_Control_Windows_App.EF.Models;

#nullable disable

namespace Goal_Achievement_Control_Windows_App.EF.Context
{
    public partial class myDBContext : DbContext
    {
        public myDBContext()
        {
        }

        public myDBContext(DbContextOptions<myDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Goal> Goals { get; set; }
        public virtual DbSet<Mark> Marks { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlite("Data Source = bin\\Debug\\netcoreapp3.1\\myDB.db");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Goal>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnType("integer")
                    .HasColumnName("id");

                entity.Property(e => e.Goal1)
                    .HasColumnType("nvarchar(250)")
                    .HasColumnName("Goal");

                entity.Property(e => e.IsMarked)
                    .IsRequired()
                    .HasColumnType("bool")
                    .HasColumnName("isMarked");

                entity.Property(e => e.UserId)
                    .HasColumnType("integer")
                    .HasColumnName("userId");
            });

            modelBuilder.Entity<Mark>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnType("integer")
                    .HasColumnName("id");

                entity.Property(e => e.Date)
                    .IsRequired()
                    .HasColumnType("nvarchar(15)");

                entity.Property(e => e.GoalId)
                    .HasColumnType("integer")
                    .HasColumnName("goal_id");

                entity.Property(e => e.Mark1)
                    .HasColumnType("nvarchar(3)")
                    .HasColumnName("mark");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnType("integer")
                    .HasColumnName("id");

                entity.Property(e => e.ChatId)
                    .IsRequired()
                    .HasColumnType("nvarchar(50)")
                    .HasColumnName("chatId");

                entity.Property(e => e.OperatingMode)
                    .IsRequired()
                    .HasColumnType("nvarchar(50)")
                    .HasColumnName("operatingMode");

                entity.Property(e => e.TelegramId)
                    .IsRequired()
                    .HasColumnType("nvarchar(50)")
                    .HasColumnName("telegramId");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
