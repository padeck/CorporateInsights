using CorporateInsights.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace CorporateInsights.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public DbSet<InsightArticle> Articles { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<InsightArticle>()
            .ToContainer("Articles")
            .HasPartitionKey(e => e.Id)
            .HasNoDiscriminator();
    }
}