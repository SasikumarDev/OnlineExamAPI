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
        public virtual DbSet<EmailTemplate> EmailTemplates { get; set; }
        public virtual DbSet<Interviewee> Interviewees { get; set; }
        public virtual DbSet<IntervieweeTest> IntervieweeTests { get; set; }
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

            modelBuilder.Entity<EmailTemplate>(entity =>
            {
                entity.HasKey(e => e.Etid)
                    .HasName("PK__EmailTem__2375175E9DC1CE23");

                entity.ToTable("EmailTemplate");

                entity.Property(e => e.Etid)
                    .HasColumnName("ETId")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.Etcreatedon)
                    .HasColumnType("datetime")
                    .HasColumnName("ETCreatedon")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Etsubject)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("ETSubject");

                entity.Property(e => e.Ettemplate).HasColumnName("ETTemplate");
            });

            modelBuilder.Entity<Interviewee>(entity =>
            {
                entity.HasKey(e => e.Iid)
                    .HasName("PK__Intervie__C4972BAC99E7BF4F");

                entity.ToTable("Interviewee");

                entity.Property(e => e.Iid)
                    .HasColumnName("IId")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.IattentPreviousTest).HasColumnName("IAttentPreviousTest");

                entity.Property(e => e.Idob)
                    .HasColumnType("datetime")
                    .HasColumnName("IDOB");

                entity.Property(e => e.IemailId)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("IEmailID");

                entity.Property(e => e.Ifname)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("IFname");

                entity.Property(e => e.Ilname)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ILname");

                entity.Property(e => e.Imobileno)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("IMobileno");
            });

            modelBuilder.Entity<IntervieweeTest>(entity =>
            {
                entity.HasKey(e => e.Itid)
                    .HasName("PKITID");

                entity.ToTable("IntervieweeTest");

                entity.Property(e => e.Itid)
                    .HasColumnName("ITID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.ItchoiceId).HasColumnName("ITChoiceID");

                entity.Property(e => e.Itiid).HasColumnName("ITIId");

                entity.Property(e => e.ItiscorrectAns).HasColumnName("ITISCorrectAns");

                entity.Property(e => e.ItqstId).HasColumnName("ITQstID");

                entity.HasOne(d => d.Itchoice)
                    .WithMany(p => p.IntervieweeTests)
                    .HasForeignKey(d => d.ItchoiceId)
                    .HasConstraintName("FKITCID");

                entity.HasOne(d => d.Iti)
                    .WithMany(p => p.IntervieweeTests)
                    .HasForeignKey(d => d.Itiid)
                    .HasConstraintName("FKITIId");

                entity.HasOne(d => d.Itqst)
                    .WithMany(p => p.IntervieweeTests)
                    .HasForeignKey(d => d.ItqstId)
                    .HasConstraintName("FKITQID");
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
