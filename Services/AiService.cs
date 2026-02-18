using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace FaceSearch.Api.Services;

public class AiService
{
    private readonly HttpClient _http;

    public AiService(HttpClient http)
    {
        _http = http;
    }

    public async Task<string> SearchFaceAsync(IFormFile file)
    {
        using var form = new MultipartFormDataContent();
        using var stream = file.OpenReadStream();

        var fileContent = new StreamContent(stream);
        fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");

        form.Add(fileContent, "file", file.FileName);

        var response = await _http.PostAsync("http://localhost:8000/search", form);
        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            // Wrap plain text error in JSON to avoid frontend parsing issues
            // Use simple string interpolation for simplicity, assuming content isn't malicious/complex JSON already
            // If content is already JSON, we might want to just return it, but "Internal Server Error" is plain text.
            // Let's try to parse it first? No, that's expensive.
            // Simple heuristic: if it doesn't start with { or [, it's likely plain text.
            if (!content.TrimStart().StartsWith("{") && !content.TrimStart().StartsWith("["))
            {
                 return System.Text.Json.JsonSerializer.Serialize(new { error = content });
            }
        }

        return content;
    }
}
