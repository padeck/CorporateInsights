using System.ClientModel;
using System.Text.Json;
using CorporateInsights.Core.Entities;
using OpenAI;
using OpenAI.Chat;

namespace CorporateInsights.Infrastructure.AI;

public class LocalAiService
{
    private readonly ChatClient _chatClient;

    public LocalAiService()
    {
        var options = new OpenAIClientOptions
        {
            Endpoint = new Uri("http://localhost:11434/v1")
        };

        _chatClient = new ChatClient("phi3", new ApiKeyCredential("dummy"), options);
    }

    public async Task<InsightArticle> EnrichArticleAsync(InsightArticle article)
    {
        Console.WriteLine($"[AI] Analysiere: '{article.OriginalTitle}'...");

        var sysMsg = new SystemChatMessage(
            "Fasse den Text in einem kurzen Satz zusammen und vergebe 2-3 passende Tags. " +
            "Antworte AUSSCHLIESSLICH im strikten JSON Format genau so: {\"summary\": \"...\", \"tags\": [\"tag1\", \"tag2\"]}"
        );
        var userMsg = new UserChatMessage(article.RawContent);

        try
        {
            var response = await _chatClient.CompleteChatAsync(new ChatMessage[] { sysMsg, userMsg });

            var responseText = response.Value.Content[0].Text;

            responseText = responseText.Replace("```json", "").Replace("```", "").Trim();

            using var doc = JsonDocument.Parse(responseText);
            article.AiSummary = doc.RootElement.GetProperty("summary").GetString() ?? "";

            foreach (var tag in doc.RootElement.GetProperty("tags").EnumerateArray())
            {
                article.Tags.Add(tag.GetString() ?? "");
            }
            Console.WriteLine("[AI] Analyse erfolgreich!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[AI Fehler] Konnte Text nicht verarbeiten. Grund: {ex.Message}");
        }

        return article;
    }
}