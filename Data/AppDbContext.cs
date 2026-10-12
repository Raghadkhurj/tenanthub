using Microsoft.EntityFrameworkCore;
using TenantHub.Models;
namespace TenantHub.Data{

public class AppDbContext:DbContext{
    
    public AppDbContext(DbContextOptions<AppDbContext> options)
    :base(options){

    }
     public DbSet<User> Users{get;set;}
   public DbSet<Tenant> Tenants{get;set;}
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Tenant>()
        .HasIndex(t => t.Slug)
        .IsUnique();
    
    modelBuilder.Entity<User>()
    .HasIndex(u =>new{ u.Email,u.TenantId})
    .IsUnique();
}
}
}