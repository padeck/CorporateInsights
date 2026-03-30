using System;
using System.Collections.Generic;

namespace CorporateInsights.Core.Entities;

public class InsightArticle
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string OriginalTitle { get; set; } = string.Empty;
    public string RawContent { get; set; } = string.Empty;
    public string AiSummary { get; set; } = string.Empty;
    public List<string> Tags { get; set; } = new List<string>();
}