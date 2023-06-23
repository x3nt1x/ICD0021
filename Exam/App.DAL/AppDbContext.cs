using App.Domain;
using App.Domain.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace App.DAL;

public class AppDbContext : IdentityDbContext<AppUser, AppRole, Guid>
{
    public DbSet<Item> Items { get; set; } = default!;
    public DbSet<Job> Jobs { get; set; } = default!;
    public DbSet<JobItem> JobItems { get; set; } = default!;
    public DbSet<Repair> Repairs { get; set; } = default!;
    public DbSet<RepairJob> RepairJobs { get; set; } = default!;
    public DbSet<AppRefreshToken> AppRefreshTokens { get; set; } = default!;

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
}