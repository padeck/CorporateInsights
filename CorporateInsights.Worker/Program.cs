using CorporateInsights.Infrastructure.AI;
using CorporateInsights.Infrastructure.Data;
using CorporateInsights.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.ServiceModel.Syndication;
using System.Xml;

Console.WriteLine("=== Smart Insights Worker (MongoDB Mode) ===\n");

var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
optionsBuilder.UseMongoDB("mongodb://localhost:27017", "InsightsDB");

using var dbContext = new AppDbContext(optionsBuilder.Options);

var aiService = new LocalAiService();
using var client = new HttpClient();
client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0");

string[] feedUrls = {
    "https://www.heise.de/rss/heise-atom.xml",
    "https://www.golem.de/rss.php?feed=RSS2.0"
};

foreach (var url in feedUrls)
{
    try
    {
        Console.WriteLine($"\n[Feed] {url}");
        var xmlContent = await client.GetStringAsync(url);
        using var stringReader = new StringReader(xmlContent);
        using var xmlReader = XmlReader.Create(stringReader);
        var feed = SyndicationFeed.Load(xmlReader);

        foreach (var item in feed.Items.Take(5))
        {
            var title = item.Title.Text.Trim();
            var content = item.Summary?.Text ?? title;

            var exists = await dbContext.Articles.AnyAsync(a => a.OriginalTitle == title);
            if (exists)
            {
                Console.WriteLine($"[Skip] {title.Substring(0, Math.Min(title.Length, 30))}...");
                continue;
            }

            var article = new InsightArticle
            {
                OriginalTitle = title,
                RawContent = content
            };

            var enriched = await aiService.EnrichArticleAsync(article);

            dbContext.Articles.Add(enriched);
            await dbContext.SaveChangesAsync();

            Console.WriteLine($"[Saved] {title}");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"[Error] {ex.Message}");
    }
}