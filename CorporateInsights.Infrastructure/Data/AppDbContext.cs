using CorporateInsights.Core.Entities;
using Microsoft.EntityFrameworkCore;
using MongoDB.EntityFrameworkCore.Extensions;

namespace CorporateInsights.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public DbSet<InsightArticle> Articles { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<InsightArticle>().ToCollection("Articles");
    }
}