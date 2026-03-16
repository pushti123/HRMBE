using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Entities;

public partial class HrmdbContext : DbContext
{
    public HrmdbContext()
    {
    }

    public HrmdbContext(DbContextOptions<HrmdbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ApplicationDeclaration> ApplicationDeclarations { get; set; }

    public virtual DbSet<ApplicationQuestion> ApplicationQuestions { get; set; }

    public virtual DbSet<Candidate> Candidates { get; set; }

    public virtual DbSet<CandidateEducation> CandidateEducations { get; set; }

    public virtual DbSet<CandidateLanguage> CandidateLanguages { get; set; }

    public virtual DbSet<CandidateResume> CandidateResumes { get; set; }

    public virtual DbSet<CandidateSkill> CandidateSkills { get; set; }

    public virtual DbSet<CandidateWorkExperience> CandidateWorkExperiences { get; set; }

    public virtual DbSet<DepartmentMst> DepartmentMsts { get; set; }

    public virtual DbSet<DesignationMst> DesignationMsts { get; set; }

    public virtual DbSet<EmployeeLeaveBalance> EmployeeLeaveBalances { get; set; }

    public virtual DbSet<Holiday> Holidays { get; set; }

    public virtual DbSet<JobApplication> JobApplications { get; set; }

    public virtual DbSet<LeaveApplication> LeaveApplications { get; set; }

    public virtual DbSet<LeaveApplicationDetail> LeaveApplicationDetails { get; set; }

    public virtual DbSet<LeavePolicy> LeavePolicies { get; set; }

    public virtual DbSet<LeaveType> LeaveTypes { get; set; }

    public virtual DbSet<PermissionMst> PermissionMsts { get; set; }

    public virtual DbSet<ResumeParserUser> ResumeParserUsers { get; set; }

    public virtual DbSet<RoleMst> RoleMsts { get; set; }

    public virtual DbSet<RolePermissionMst> RolePermissionMsts { get; set; }

    public virtual DbSet<TicketMst> TicketMsts { get; set; }

    public virtual DbSet<TokenMst> TokenMsts { get; set; }

    public virtual DbSet<UserMst> UserMsts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=DESKTOP-S4I2BKN\\SQLEXPRESS;Initial Catalog=HRMDB;Integrated Security=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ApplicationDeclaration>(entity =>
        {
            entity.HasKey(e => e.DeclarationId).HasName("PK__Applicat__B4AA37DF8502E6E4");

            entity.Property(e => e.SubmittedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<ApplicationQuestion>(entity =>
        {
            entity.HasKey(e => e.QuestionId).HasName("PK__Applicat__0DC06FAC1EC2AC49");

            entity.Property(e => e.ExpectedCtc)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("ExpectedCTC");
            entity.Property(e => e.ReferralName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Source)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Candidate>(entity =>
        {
            entity.HasKey(e => e.CandidateId).HasName("PK__Candidat__DF539B9CFB528FBB");

            entity.Property(e => e.CountryCode)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.FinalSubmitted).HasDefaultValue(false);
            entity.Property(e => e.FirstName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.IsActive).HasDefaultValue(false);
            entity.Property(e => e.LastName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Mobile)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<CandidateEducation>(entity =>
        {
            entity.HasKey(e => e.EducationId).HasName("PK__Candidat__4BBE38053552EB0B");

            entity.Property(e => e.CollegeName)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Degree)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FieldOfStudy)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<CandidateLanguage>(entity =>
        {
            entity.HasKey(e => e.LanguageId).HasName("PK__Candidat__B93855AB8120B787");

            entity.Property(e => e.LanguageName)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<CandidateResume>(entity =>
        {
            entity.HasKey(e => e.ResumeId).HasName("PK__Candidat__D7D7A0F726441EF2");

            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FileName)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.FilePath)
                .HasMaxLength(500)
                .IsUnicode(false);
        });

        modelBuilder.Entity<CandidateSkill>(entity =>
        {
            entity.HasKey(e => e.SkillId).HasName("PK__Candidat__DFA0918715C38E52");

            entity.Property(e => e.SkillName)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<CandidateWorkExperience>(entity =>
        {
            entity.HasKey(e => e.WorkId).HasName("PK__Candidat__2DE6D5F54BB0FDFE");

            entity.Property(e => e.CandidateWorkExperiencesJobTitle)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.CompanyName)
                .HasMaxLength(200)
                .IsUnicode(false);
        });

        modelBuilder.Entity<DepartmentMst>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Departme__3214EC077834098C");

            entity.ToTable("DepartmentMst");

            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.DepartmentName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
        });

        modelBuilder.Entity<DesignationMst>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Designat__3214EC078294FEF0");

            entity.ToTable("DesignationMst");

            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.DesignationName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
        });

        modelBuilder.Entity<EmployeeLeaveBalance>(entity =>
        {
            entity.HasKey(e => e.BalanceId).HasName("PK__Employee__A760D5BEFF2E76C6");

            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UsedDays).HasDefaultValue(0);

            entity.HasOne(d => d.LeaveType).WithMany(p => p.EmployeeLeaveBalances)
                .HasForeignKey(d => d.LeaveTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Balance_LeaveType");
        });

        modelBuilder.Entity<Holiday>(entity =>
        {
            entity.HasKey(e => e.HolidayId).HasName("PK__Holidays__2D35D57AD50BE26C");

            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.HolidayName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.IsDelete).HasDefaultValue(false);
            entity.Property(e => e.UpdatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<JobApplication>(entity =>
        {
            entity.HasKey(e => e.JobApplicationId).HasName("PK__JobAppli__BD557F85C7E2C27D");

            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FinalSubmitted).HasDefaultValue(false);
            entity.Property(e => e.IsActive).HasDefaultValue(false);
            entity.Property(e => e.JobTitle)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<LeaveApplication>(entity =>
        {
            entity.HasKey(e => e.LeaveApplicationId).HasName("PK__LeaveApp__038EC26DC1C8FECA");

            entity.Property(e => e.AppliedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ApprovedDate).HasColumnType("datetime");
            entity.Property(e => e.LwpDays).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.PaidLeaveDays).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.Reason)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("Pending");
            entity.Property(e => e.TotalDays).HasColumnType("decimal(4, 1)");

            entity.HasOne(d => d.LeaveType).WithMany(p => p.LeaveApplications)
                .HasForeignKey(d => d.LeaveTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LeaveApplication_LeaveType");
        });

        modelBuilder.Entity<LeaveApplicationDetail>(entity =>
        {
            entity.HasKey(e => e.LeaveDetailId).HasName("PK__LeaveApp__D31ED2B5678EF2B3");

            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.LeaveDayType)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.LeaveApplication).WithMany(p => p.LeaveApplicationDetails)
                .HasForeignKey(d => d.LeaveApplicationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LeaveApplicationDetails");
        });

        modelBuilder.Entity<LeavePolicy>(entity =>
        {
            entity.HasKey(e => e.LeavePolicyId).HasName("PK__LeavePol__20E59E58DE0646D2");

            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.IsCarryForward).HasDefaultValue(false);
            entity.Property(e => e.IsDelete).HasDefaultValue(false);
            entity.Property(e => e.IsHalfDayAllowed).HasDefaultValue(false);
            entity.Property(e => e.IsHolidaySandwichApplicable).HasDefaultValue(false);
            entity.Property(e => e.IsProbationApplicable).HasDefaultValue(false);
            entity.Property(e => e.IsSandwichApplicable).HasDefaultValue(false);
            entity.Property(e => e.IsWeelOffSandwichApplicable).HasDefaultValue(false);
            entity.Property(e => e.LeaveCreditType)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.LeaveType).WithMany(p => p.LeavePolicies)
                .HasForeignKey(d => d.LeaveTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LeavePolicy_LeaveType");
        });

        modelBuilder.Entity<LeaveType>(entity =>
        {
            entity.HasKey(e => e.LeaveTypeId).HasName("PK__LeaveTyp__43BE8F14EF493038");

            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.IsDelete).HasDefaultValue(false);
            entity.Property(e => e.LeaveCode)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.LeaveName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<PermissionMst>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Permissi__3214EC07F7B7746E");

            entity.ToTable("PermissionMst");

            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.PermissionDescription)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.PermissionName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
        });

        modelBuilder.Entity<ResumeParserUser>(entity =>
        {
            entity.HasKey(e => e.ResumeParserId).HasName("PK__ResumePa__4557B8915226C0CE");

            entity.ToTable("ResumeParserUser");

            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Password).HasMaxLength(50);
        });

        modelBuilder.Entity<RoleMst>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RoleMst__3214EC0756435C15");

            entity.ToTable("RoleMst");

            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.RoleName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
        });

        modelBuilder.Entity<RolePermissionMst>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RolePerm__3214EC07ADD917D7");

            entity.ToTable("RolePermissionMst");

            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
        });

        modelBuilder.Entity<TicketMst>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TicketMs__3214EC07A38FAA0D");

            entity.ToTable("TicketMst");

            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Subject)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
        });

        modelBuilder.Entity<TokenMst>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TokenMst__3214EC075CCE5C54");

            entity.ToTable("TokenMst");

            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.RefreshTokenExpiryDate).HasColumnType("datetime");
            entity.Property(e => e.TokenExpiryDate).HasColumnType("datetime");
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
        });

        modelBuilder.Entity<UserMst>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UserMst__3214EC079583F8A3");

            entity.ToTable("UserMst");

            entity.Property(e => e.AadharNumber)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.BankName).IsUnicode(false);
            entity.Property(e => e.BankNumber)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Contact)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Fullname)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Gender)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Pincode).HasMaxLength(8);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
            entity.Property(e => e.UserId)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
