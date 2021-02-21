using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace OnlineExamAPI.OnlineExamModels
{
    public partial class OnlineExamContext : DbContext
    {
        public OnlineExamContext()
        {
        }

        public OnlineExamContext(DbContextOptions<OnlineExamContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AdminUser> AdminUsers { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Choice> Choices { get; set; }
        public virtual DbSet<Question> Questions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=localhost;User ID = SA;Password = Sa.123456789;Database=MiniOnlineExam;Trusted_Connection=False;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<AdminUser>(entity =>
            {
                entity.HasKey(e => e.AdminUsId)
                    .HasName("PK__AdminUse__88534E0F028D1A79");

                entity.Property(e => e.AdminUsId)
                    .HasColumnName("AdminUsID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.Apassword)
                    .HasMaxLength(120)
                    .HasColumnName("APASSWORD");

                entity.Property(e => e.CreatedOn)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DateofBirth).HasColumnType("datetime");

                entity.Property(e => e.EmailId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("EmailID");

                entity.Property(e => e.Firstname)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ImageUrl)
                    .IsRequired()
                    .HasMaxLength(300);

                entity.Property(e => e.Lastname)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.UpdateOn)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.Cid)
                    .HasName("PK__Category__C1F8DC59D38568F1");

                entity.ToTable("Category");

                entity.Property(e => e.Cid)
                    .HasColumnName("CID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.CdefaultScoreorQst).HasColumnName("CDefaultScoreorQst");

                entity.Property(e => e.Cdesc)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("CDesc");

                entity.Property(e => e.CnoOfQst).HasColumnName("CNoOfQst");

                entity.Property(e => e.CqstType).HasColumnName("CQstType");

                entity.Property(e => e.Cstatus)
                    .HasColumnName("CStatus")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.CtotalQst).HasColumnName("CTotalQst");
            });

            modelBuilder.Entity<Choice>(entity =>
            {
                entity.HasKey(e => e.Cid)
                    .HasName("PK__Choices__C1F8DC59AA1064CE");

                entity.Property(e => e.Cid)
                    .HasColumnName("CID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.CcreatedOn)
                    .HasColumnType("datetime")
                    .HasColumnName("CCreatedON")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Ciscrt).HasColumnName("CISCRT");

                entity.Property(e => e.Cstatus)
                    .HasColumnName("CSTATUS")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Ctext)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CTEXT");

                entity.Property(e => e.CupdateOn)
                    .HasColumnType("datetime")
                    .HasColumnName("CUpdateON")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Qid).HasColumnName("QID");

                entity.HasOne(d => d.QidNavigation)
                    .WithMany(p => p.Choices)
                    .HasForeignKey(d => d.Qid)
                    .HasConstraintName("FK_QID");
            });

            modelBuilder.Entity<Question>(entity =>
            {
                entity.HasKey(e => e.Qid)
                    .HasName("PK__Question__CAB147CBD8238A2F");

                entity.ToTable("Question");

                entity.Property(e => e.Qid)
                    .HasColumnName("QID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.Qcid).HasColumnName("QCID");

                entity.Property(e => e.QcreatedOn)
                    .HasColumnType("datetime")
                    .HasColumnName("QCreatedON")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Qstatus).HasColumnName("QStatus");

                entity.Property(e => e.Qtext)
                    .IsRequired()
                    .HasMaxLength(500)
                    .HasColumnName("QText");

                entity.Property(e => e.QupdateOn)
                    .HasColumnType("datetime")
                    .HasColumnName("QUpdateON")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Qc)
                    .WithMany(p => p.Questions)
                    .HasForeignKey(d => d.Qcid)
                    .HasConstraintName("FK_CID");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
