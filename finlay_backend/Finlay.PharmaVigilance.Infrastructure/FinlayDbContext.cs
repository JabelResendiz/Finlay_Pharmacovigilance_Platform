

using Finlay.PharmaVigilance.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Finlay.PharmaVigilance.Infrastructure;


public class FinlayDbContext : IdentityDbContext<User,Role,int>
{
    public FinlayDbContext(DbContextOptions options) : base(options) {

    }


    public DbSet<Employee> Employees {get;set;}


    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
        });


        builder.Entity<Role>(entity => 
        {
            entity.HasKey(e => e.Id);
        });

        builder.Entity<Employee>(entity => 
        {
            entity.HasKey(e => e.Id);
            entity.Property(e=> e.Name)
                  .IsRequired();
            entity.Property(e=> e.UserRole)
                  .IsRequired();
            entity.Property(e=> e.Email)
                  .IsRequired();
            entity.Property(e=> e.UserName)
                  .IsRequired();
        });


    }
}