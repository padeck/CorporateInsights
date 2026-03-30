using CorporateInsights.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CorporateInsights.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InsightsController : ControllerBase
{
    private readonly AppDbContext _context;

    public InsightsController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/insights (Alle Artikel laden)
    [HttpGet]
    public async Task<IActionResult> GetArticles()
    {
        var articles = await _context.Articles
            .OrderByDescending(a => a.Id) // In Cosmos ist Id oft an Zeit gekoppelt oder wir nehmen ein Datum
            .ToListAsync();
        return Ok(articles);
    }

    // GET: api/insights/search?tag=AI
    [HttpGet("search")]
    public async Task<IActionResult> SearchByTag([FromQuery] string tag)
    {
        // Einfache Suche in den Tags
        var articles = await _context.Articles
            .Where(a => a.Tags.Contains(tag))
            .ToListAsync();
        return Ok(articles);
    }
}