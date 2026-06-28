using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Test.DataModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BCrypt.Net;
using System.Net.Http.Headers;   // ✅ AuthenticationHeaderValue
using System.Text;               // ✅ Encoding
using System.Text.Json;          // ✅ JsonSerializer, JsonDocument
[ApiController]
[Route("api/AIChat")]
public class ChatController : ControllerBase
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public ChatController(IHttpClientFactory httpClientFactory, IOptions<OpenRouterSettings> settings)
    {
        _httpClient = httpClientFactory.CreateClient();
        _httpClient.Timeout = TimeSpan.FromMinutes(5);
        _apiKey = settings.Value.ApiKey;
    }

    [HttpPost("Chat")]
    public async Task<IActionResult> GetResult(string prompt)
    {
        Console.WriteLine(prompt);
        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", _apiKey);

        _httpClient.DefaultRequestHeaders.Add("HTTP-Referer", "http://localhost:4200");
        _httpClient.DefaultRequestHeaders.Add("X-Title", "MF App");

        var requestBody = new
        {
            model = "nvidia/nemotron-3-super-120b-a12b:free",
            messages = new[]
    {
        new
        {
            role = "system",
            content = @"
You are a financial assistant.

Return output ONLY in clean HTML format.

Rules:
- No <html>, <body> tags
- Use only div, span, p, h3
- Use class names exactly:
  insights-card, insight-item, label, value, positive, neutral, low
- No explanations, no markdown, no extra text
- Keep it short and UI friendly
"
        },
        new
        {
            role = "user",
            content = prompt
        }
    }
        };

        var content = new StringContent(
            JsonSerializer.Serialize(requestBody),
            Encoding.UTF8,
            "application/json"
        );

        var response = await _httpClient.PostAsync(
            "https://openrouter.ai/api/v1/chat/completions",
            content
        );

        var responseString = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            return BadRequest(responseString);

        using var doc = JsonDocument.Parse(responseString);
        var root = doc.RootElement;
        Console.WriteLine(root);
        // ✅ Check if "choices" exists
        if (root.TryGetProperty("choices", out var choices) &&
            choices.GetArrayLength() > 0 &&
            choices[0].TryGetProperty("message", out var message) &&
            message.TryGetProperty("content", out var contents))
        {
            var result = contents.GetString();
            return Ok(result);
        }

        // ✅ Handle error response
        if (root.TryGetProperty("error", out var error))
        {
            var errorMsg = error.TryGetProperty("message", out var msg)
                ? msg.GetString()
                : "Unknown AI error";

            return BadRequest($"AI Error: {errorMsg}");
        }

        // ✅ fallback
        return BadRequest("Invalid AI response format");

    }


    [HttpPost("CompareFundsAIAnalysis")]
    public async Task<IActionResult> CompareFunds([FromBody] FundComparisonDto comparison)
    {
        var prompt = $@"
Compare the following mutual funds and provide:

1. Performance Comparison
2. Strengths
3. Weaknesses
4. Long-Term Suitability
5. Risk Assessment
6. Overall Winner

Fund 1:
Name: {comparison.Fund1.SchemeName}
Fund House: {comparison.Fund1.FundHouse}
Category: {comparison.Fund1.Category}
Type: {comparison.Fund1.Type}
Latest NAV: {comparison.Fund1.LatestNav}
Latest NAV Date: {comparison.Fund1.LatestNavDate}
Return: {comparison.Fund1.ReturnPercentage}%
Records: {comparison.Fund1.TotalRecords}

Fund 2:
Name: {comparison.Fund2.SchemeName}
Fund House: {comparison.Fund2.FundHouse}
Category: {comparison.Fund2.Category}
Type: {comparison.Fund2.Type}
Latest NAV: {comparison.Fund2.LatestNav}
Latest NAV Date: {comparison.Fund2.LatestNavDate}
Return: {comparison.Fund2.ReturnPercentage}%
Records: {comparison.Fund2.TotalRecords}

Current Winner:
{comparison.Winner.SchemeName}
Return: {comparison.Winner.ReturnPercentage}%

Return ONLY clean HTML.
";

        _httpClient.DefaultRequestHeaders.Clear();

        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", _apiKey);

        _httpClient.DefaultRequestHeaders.Add(
            "HTTP-Referer",
            "http://localhost:4200");

        _httpClient.DefaultRequestHeaders.Add(
            "X-Title",
            "MF App");

        var requestBody = new
        {
            model = "nvidia/nemotron-3-super-120b-a12b:free",
            messages = new[]
            {
            new
            {
                role = "system",
                content = @"
You are a mutual fund comparison assistant.

Return output ONLY in clean HTML format.

Rules:
- No html/body tags
- Use only div, span, p, h3
- Use classes:
  insights-card,
  insight-item,
  label,
  value,
  positive,
  neutral,
  low
- Keep response concise
- Highlight the better performing fund
- Mention risks and suitability
- No markdown
"
            },
            new
            {
                role = "user",
                content = prompt
            }
        }
        };

        var content = new StringContent(
            JsonSerializer.Serialize(requestBody),
            Encoding.UTF8,
            "application/json"
        );

        var response = await _httpClient.PostAsync(
            "https://openrouter.ai/api/v1/chat/completions",
            content
        );

        var responseString =
            await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            return BadRequest(responseString);

        using var doc =
            JsonDocument.Parse(responseString);

        var root = doc.RootElement;

        if (root.TryGetProperty("choices", out var choices) &&
            choices.GetArrayLength() > 0 &&
            choices[0].TryGetProperty("message", out var message) &&
            message.TryGetProperty("content", out var contents))
        {
            return Ok(contents.GetString());
        }

        if (root.TryGetProperty("error", out var error))
        {
            var errorMessage =
                error.TryGetProperty("message", out var msg)
                    ? msg.GetString()
                    : "Unknown AI error";

            return BadRequest($"AI Error: {errorMessage}");
        }

        return BadRequest("Invalid AI response format");
    }
}