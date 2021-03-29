using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Project.Model.DataModels
{
    public partial class OnlineTestContext : DbContext
    {
        public OnlineTestContext()
        {
        }

        public OnlineTestContext(DbContextOptions<OnlineTestContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Exam> Exams { get; set; }
        public virtual DbSet<ExamQuestion> ExamQuestions { get; set; }
        public virtual DbSet<ExamSubSection> ExamSubSections { get; set; }
        public virtual DbSet<ExamUser> ExamUsers { get; set; }
        public virtual DbSet<ExamUserAnswer> ExamUserAnswers { get; set; }
        public virtual DbSet<Question> Questions { get; set; }
        public virtual DbSet<QuestionOption> QuestionOptions { get; set; }
        public virtual DbSet<Subject> Subjects { get; set; }
        public virtual DbSet<SubjectQuestion> SubjectQuestions { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ModelCreatingConfiguration();

            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Exam>(entity =>
            {
                entity.Property(e => e.CreationDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.IsDeleted)
                    .IsRequired()
                    .HasDefaultValueSql("(N'false')");

                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.Uid).HasDefaultValueSql("(newid())");
            });

            modelBuilder.Entity<ExamQuestion>(entity =>
            {
                entity.HasOne(d => d.Exam)
                    .WithMany(p => p.ExamQuestions)
                    .HasForeignKey(d => d.ExamId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ExamQuestions_Exams");

                entity.HasOne(d => d.Question)
                    .WithMany(p => p.ExamQuestions)
                    .HasForeignKey(d => d.QuestionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ExamQuestions_Questions");

                entity.HasOne(d => d.SubSection)
                    .WithMany(p => p.ExamQuestions)
                    .HasForeignKey(d => d.SubSectionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ExamQuestions_ExamSubSections");
            });

            modelBuilder.Entity<ExamSubSection>(entity =>
            {
                entity.Property(e => e.Description).HasMaxLength(300);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasOne(d => d.Exam)
                    .WithMany(p => p.ExamSubSections)
                    .HasForeignKey(d => d.ExamId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ExamSubSections_Exams");
            });

            modelBuilder.Entity<ExamUser>(entity =>
            {
                entity.Property(e => e.ExamEndDate).HasColumnType("datetime");

                entity.Property(e => e.ExamStartDate).HasColumnType("datetime");

                entity.HasOne(d => d.Exam)
                    .WithMany(p => p.ExamUsers)
                    .HasForeignKey(d => d.ExamId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ExamUsers_Exams");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ExamUsers)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ExamUsers_Users");
            });

            modelBuilder.Entity<ExamUserAnswer>(entity =>
            {
                entity.Property(e => e.BlankQuestionAnswer).HasMaxLength(300);

                entity.HasOne(d => d.ExamQuestion)
                    .WithMany(p => p.ExamUserAnswers)
                    .HasForeignKey(d => d.ExamQuestionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ExamUserAnswers_ExamQuestions");

                entity.HasOne(d => d.ExamUser)
                    .WithMany(p => p.ExamUserAnswers)
                    .HasForeignKey(d => d.ExamUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ExamUserAnswers_ExamUsers");

                entity.HasOne(d => d.SelectedOption)
                    .WithMany(p => p.ExamUserAnswers)
                    .HasForeignKey(d => d.SelectedOptionId)
                    .HasConstraintName("FK_ExamUserAnswers_QuestionOptions");
            });

            modelBuilder.Entity<Question>(entity =>
            {
                entity.Property(e => e.BlankQuestionAnswer).HasMaxLength(100);

                entity.Property(e => e.CreationDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DescriptiveAnswer).IsRequired();

                entity.Property(e => e.IsDeleted)
                    .IsRequired()
                    .HasDefaultValueSql("(N'false')");

                entity.Property(e => e.ModificationDate).HasColumnType("datetime");

                entity.Property(e => e.Question1)
                    .IsRequired()
                    .HasColumnName("Question");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Uid).HasDefaultValueSql("(newid())");
            });

            modelBuilder.Entity<QuestionOption>(entity =>
            {
                entity.Property(e => e.IsDeleted)
                    .IsRequired()
                    .HasDefaultValueSql("(N'false')");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.Uid).HasDefaultValueSql("(newid())");

                entity.HasOne(d => d.Question)
                    .WithMany(p => p.QuestionOptions)
                    .HasForeignKey(d => d.QuestionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_QuestionOptions_Questions");
            });

            modelBuilder.Entity<Subject>(entity =>
            {
                entity.Property(e => e.Description).HasMaxLength(300);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasOne(d => d.Parent)
                    .WithMany(p => p.InverseParent)
                    .HasForeignKey(d => d.ParentId)
                    .HasConstraintName("FK_Subjects_Subjects1");
            });

            modelBuilder.Entity<SubjectQuestion>(entity =>
            {
                entity.HasOne(d => d.Question)
                    .WithMany(p => p.SubjectQuestions)
                    .HasForeignKey(d => d.QuestionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SubjectQuestions_Questions");

                entity.HasOne(d => d.Subject)
                    .WithMany(p => p.SubjectQuestions)
                    .HasForeignKey(d => d.SubjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SubjectQuestions_Subjects");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.Username, "UX_Users_Username")
                    .IsUnique();

                entity.Property(e => e.CreationDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Firstname).HasMaxLength(100);

                entity.Property(e => e.IsDeleted)
                    .IsRequired()
                    .HasDefaultValueSql("(N'false')");

                entity.Property(e => e.Lastname).HasMaxLength(100);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Stamp).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
