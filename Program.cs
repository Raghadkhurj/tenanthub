using Microsoft.EntityFrameworkCore;
using TenantHub.Data;
using TenantHub.Models;
DotNetEnv.Env.TraversePath().Load();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
 

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    if (!db.Admins.Any())
    {
        db.Admins.Add(new Admin {Email = builder.Configuration["ADMIN_EMAIL"],
        PasswordHash = BCrypt.Net.BCrypt.HashPassword(builder.Configuration["ADMIN_PASSWORD"]) });
        await db.SaveChangesAsync();
    }
}
    

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
