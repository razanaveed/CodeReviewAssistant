
using CodeReviewAssistant.Models;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
namespace CodeReviewAssistant.Services;

public class OpenAIService
{
    private readonly HttpClient _httpClient;

    public OpenAIService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    private string BuildSystemPrompt(UserAISettings s)
    {
        return $@"
You are a Code Review Assistant.

Rules:
- Provide exactly {s.ImprovementCount} improvements
- Provide exactly {s.PositiveCount} positive note(s)
- Keep responses concise
{(s.IncludeRefactoredCode ? "- Include refactored code" : "")}

Format:

Improvements:

1.
2.
...

Positive:

-

{(s.IncludeRefactoredCode ? "Refactored Code:" : "")}

";
    }

    private string BuildUserPrompt(string code, UserAISettings s)
    {
        return $@"
Please review the following code:

Code:
{code}
";
    }


    public async Task<string> ReviewCodeAsync(string code, UserAISettings settings)
    {
        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", settings.ApiKey);
        var url = "https://api.groq.com/openai/v1/chat/completions";

        var body = new
        {
            model = settings.Model,
            temperature = settings.Temperature,
            max_tokens = settings.MaxTokens,
            messages = new[]
            {
                // SYSTEM MESSAGE
                new { role = "system", content = BuildSystemPrompt(settings) },

                // USER MESSAGE
                new { role = "user", content = BuildUserPrompt(code, settings) }
            }
        };
        try
        {
            var httpRequest = new HttpRequestMessage(HttpMethod.Post, url);
            httpRequest.Content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(httpRequest);
            var json = await response.Content.ReadAsStringAsync();

            // 🔥 HANDLE ERRORS FIRST
            if (!response.IsSuccessStatusCode)
            {
                try
                {
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    var errorObj = JsonSerializer.Deserialize<OpenAIErrorResponse>(json, options);

                    if (errorObj?.Error != null)
                    {
                        return $"ERROR::{errorObj.Error.Code}::{errorObj.Error.Message}";
                    }
                }
                catch { }

                return "ERROR::unknown::Something went wrong with API call";
            }

            var content = ExtractContent(json);
            return content;

        }
        catch (Exception)
        {

            throw;
        }

    }
    private string ExtractContent(string json)
    {
        if (string.IsNullOrWhiteSpace(json))
            return "";

        var response = JsonSerializer.Deserialize<GroqChatResponse>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        // Take first assistant message
        var content = response?.Choices?.FirstOrDefault()?.Message?.Content;

        return content ?? "";
    }
}
