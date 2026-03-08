

using Finlay.PharmaVigilance.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Finlay.PharmaVigilance.Infrastructure;


public class FinlayDbContext : IdentityDbContext<User, Role, int>
{
  public FinlayDbContext(DbContextOptions options) : base(options)
  {

  }

  public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
  {
    var entriesGeneric = ChangeTracker.Entries<GenericEntity>();

    foreach (var entry in entriesGeneric)
    {
      if (entry.State == EntityState.Added)
      {
        entry.Entity.CreatedAt = DateTime.UtcNow;
        entry.Entity.UpdatedAt = DateTime.UtcNow;
      }

      if (entry.State == EntityState.Modified)
      {
        entry.Entity.UpdatedAt = DateTime.UtcNow;
      }
    }

    var entriesUser = ChangeTracker.Entries<User>();

    foreach (var entry in entriesUser)
    {
      if (entry.State == EntityState.Added)
      {
        entry.Entity.CreatedAt = DateTime.UtcNow;
        entry.Entity.UpdatedAt = DateTime.UtcNow;
      }

      if (entry.State == EntityState.Modified)
      {
        entry.Entity.UpdatedAt = DateTime.UtcNow;
      }
    }

    return await base.SaveChangesAsync(cancellationToken);
  }


  //public DbSet<Employee> Employees {get;set;}
  public DbSet<AdverseEvent> AdverseEvents { get; set; }
  public DbSet<AefiReport> AefiReport { get; set; }
  public DbSet<Patient> Patients { get; set; }
  public DbSet<Physician> Physicians { get; set; }
  public DbSet<Symptom> Symptoms { get; set; }
  public DbSet<Vaccination> Vaccinations { get; set; }
  public DbSet<Vaccine> Vaccines { get; set; }
  public DbSet<AdverseEventSymptom> AdverseEventSymptoms { get; set; }

  protected override void OnModelCreating(ModelBuilder builder)
  {
    base.OnModelCreating(builder);

    builder.Entity<User>(entity =>
    {
      entity.HasKey(e => e.Id);
      entity.Property(e => e.UserRole)
                .IsRequired();
      entity.Property(e => e.CreatedAt)
                .IsRequired();
      entity.Property(e => e.UpdatedAt)
                .IsRequired();
      // entity.Property(e=> e.RefreshToken)
      //       .IsRequired();
    });


    builder.Entity<Role>(entity =>
    {
      entity.HasKey(e => e.Id);

    });

    builder.Entity<AdverseEvent>(entity =>
    {
      entity.HasKey(e => e.Id);
      entity.Property(e => e.StartDate)
                .IsRequired();
      entity.Property(e => e.Description)
                .IsRequired();
      entity.Property(e => e.Severity)
                .IsRequired();
      entity.Property(e => e.RequiredHospitalization)
                .IsRequired();
      entity.Property(e => e.Notes)
                .IsRequired();
      entity.Property(e => e.Treatment)
                .IsRequired();
      entity.Property(e => e.CurrentStatus)
                .IsRequired();

      entity.HasOne(e => e.AefiReport)
            .WithMany(r => r.AdverseEvents)
            .HasForeignKey(e => e.AefiReportId)
            .OnDelete(DeleteBehavior.Cascade);

    });

    builder.Entity<AdverseEventSymptom>(entity =>
  {

    entity.HasKey(e => new { e.AdverseEventId, e.SymptomId });

    entity.HasOne(e => e.AdverseEvent)
          .WithMany(r => r.AdverseEventSymptoms)
          .HasForeignKey(e => e.AdverseEventId);

    entity.HasOne(e => e.Symptom)
         .WithMany(r => r.AdverseEventSymptoms)
         .HasForeignKey(e => e.SymptomId);


    // entity.HasKey(e => e.Id);
    // entity.HasIndex(e => new { e.AdverseEventId, e.SymptomId })
    // .IsUnique();

  });

    builder.Entity<AefiReport>(entity =>
    {
      entity.HasKey(e => e.Id);
      entity.Property(e => e.ReportDate)
                .IsRequired();
      entity.Property(e => e.GeneralNotes)
                .IsRequired();

      entity.HasOne(r => r.Patient)
            .WithMany(p => p.AefiReports)
            .HasForeignKey(r => r.PatientId)
            .OnDelete(DeleteBehavior.Restrict);

      entity.HasOne(r => r.Physician)
          .WithMany(p => p.AefiReports)
          .HasForeignKey(r => r.PhysicianId)
          .OnDelete(DeleteBehavior.Restrict);

      entity.HasOne(r => r.Vaccination)
            .WithMany(v => v.AefiReports)
            .HasForeignKey(r => r.VaccinationId)
            .OnDelete(DeleteBehavior.Restrict);


    });

    builder.Entity<Patient>(entity =>
    {
      entity.HasKey(p => p.Id);

      entity.Property(p => p.FullName)
          .IsRequired()
          .HasMaxLength(150);

      entity.Property(p => p.Address)
          .IsRequired()
          .HasMaxLength(250);

      entity.Property(p => p.Age)
          .IsRequired();

      entity.Property(p => p.DateOfBirth)
          .IsRequired();


      entity.Property(p => p.Gender)
          .IsRequired()
          .HasConversion<string>();

      entity.Property(p => p.Province)
          .IsRequired()
          .HasConversion<string>();
    });

    builder.Entity<Physician>(entity =>
    {
      entity.HasKey(p => p.Id);

      entity.Property(p => p.FullName)
          .IsRequired()
          .HasMaxLength(150);

      entity.Property(p => p.MedicalHistory)
          .IsRequired()
          .HasMaxLength(1000);

      entity.Property(p => p.DateOfBirth)
          .IsRequired();


      entity.Property(p => p.Gender)
          .IsRequired()
          .HasConversion<string>();

    });

    builder.Entity<Symptom>(entity =>
    {
      entity.HasKey(p => p.Id);

      entity.Property(p => p.Name)
          .IsRequired()
          .HasMaxLength(120);

      entity.Property(p => p.Description)
          .IsRequired()
          .HasMaxLength(800);

      entity.Property(p => p.StandardCode)
          .IsRequired()
          .HasMaxLength(30);



    });

    builder.Entity<Vaccination>(entity =>
    {
      entity.HasKey(e => e.Id);

      entity.Property(e => e.BatchNumber)
            .IsRequired()
            .HasMaxLength(50);

      entity.Property(e => e.AdministrationSite)
            .IsRequired()
            .HasMaxLength(100);

      entity.Property(e => e.DoseNumber)
            .IsRequired();


      entity.HasOne(v => v.Vaccine)
            .WithMany(v => v.Vaccinations)
            .HasForeignKey(v => v.VaccineId)
            .OnDelete(DeleteBehavior.Restrict);

    });

    builder.Entity<Vaccine>(entity =>
    {
      entity.HasKey(e => e.Id);

      entity.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(150);

      entity.Property(e => e.Manufacturer)
            .IsRequired()
            .HasMaxLength(150);

      entity.Property(e => e.VaccineType)
            .IsRequired()
            .HasMaxLength(50);

      entity.Property(e => e.Description)
            .HasMaxLength(1000);


    });



  }
}