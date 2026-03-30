using CorporateInsights.Infrastructure.AI;
using CorporateInsights.Infrastructure.Data;
using CorporateInsights.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Azure.Cosmos;

Console.WriteLine("=== Corporate Insights Worker startet (Cosmos DB Edition) ===\n");

var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
optionsBuilder.UseCosmos(
    accountEndpoint: "http://localhost:8081",
    accountKey: "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==",
    databaseName: "InsightsDB",
    cosmosOptions =>
    {
        cosmosOptions.ConnectionMode(ConnectionMode.Gateway);
    }
);

using var dbContext = new AppDbContext(optionsBuilder.Options);

Console.WriteLine("[DB] Prüfe/Erstelle Datenbank-Struktur in Cosmos DB...");
await dbContext.Database.EnsureCreatedAsync();
Console.WriteLine("[DB] Datenbank 'InsightsDB' und Container 'Articles' sind bereit.");

var aiService = new LocalAiService();

var article = new InsightArticle
{
    OriginalTitle = "Apple kündigt neue KI-Features an",
    RawContent = "Apple hat heute auf der Entwicklerkonferenz neue Funktionen für maschinelles Lernen vorgestellt. " +
                 "Diese sollen tief in iOS integriert werden und den Datenschutz wahren."
};

var enriched = await aiService.EnrichArticleAsync(article);

Console.WriteLine("[DB] Speichere Artikel in Cosmos DB...");
dbContext.Articles.Add(enriched);
await dbContext.SaveChangesAsync();

Console.WriteLine("\n[ERFOLG] Artikel wurde analysiert und in Cosmos DB gespeichert!");