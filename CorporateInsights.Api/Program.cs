using CorporateInsights.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Azure.Cosmos;

var builder = WebApplication.CreateBuilder(args);

// 1. Services registrieren
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 2. Datenbank
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseCosmos(
        "http://localhost:8081",
        "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==",
        "InsightsDB",
        cosmosOptions =>
        {
            cosmosOptions.ConnectionMode(ConnectionMode.Gateway);
        }
    );
});

// 3. CORS Policy definieren
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// 4. Middleware Pipeline - DIE REIHENFOLGE IST KRITISCH
app.UseSwagger();
app.UseSwaggerUI();

// CORS muss VOR MapControllers stehen
app.UseCors("AllowAll");

app.MapControllers();

app.Run();