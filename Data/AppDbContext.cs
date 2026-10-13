using Microsoft.EntityFrameworkCore;
using TenantHub.Models;
namespace TenantHub.Data{

public class AppDbContext:DbContext{
    
    public AppDbContext(DbContextOptions<AppDbContext> options)
    :base(options){

    }
     public DbSet<User> Users{get;set;}
     public DbSet<Tenant> Tenants{get;set;}
     public DbSet<Admin> Admins{get;set;}

protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Tenant>()
        .HasIndex(t => t.Slug)
        .IsUnique();
    
    modelBuilder.Entity<User>()
    .HasIndex(u =>new{ u.Email,u.TenantId})
    .IsUnique();
    modelBuilder.Entity<Admin>()
        .HasIndex(a => a.Email)
        .IsUnique();
    modelBuilder.Entity<Tenant>().HasData(
        new Tenant{Id=1,Name="Innovation grid",Slug="innovation-grid"},
        new Tenant{Id=2,Name="Orange Company",Slug="orange-company"}

    );

}    

}
}