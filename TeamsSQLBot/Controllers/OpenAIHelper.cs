using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

public class OpenAIHelper
{
    private static readonly HttpClient client = new HttpClient();
    private const string openAiEndpoint = "";
    private const string apiKey = "";

    public async Task<string> CallOpenAIToGenerateSQL(string userQuery)
    {
        var requestBody = new
        {
            messages = new[]
            {
                    new { role = "system", content = "Genera solo una query SQL Server valida. Non aggiungere testo esplicativo o commenti." },
                    new { role = "user", content = $"Converti questa richiesta in SQL: {userQuery}" }
                },
            max_tokens = 100,
            temperature = 0,
            stop = new[] { ";" }
        };

        var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
        client.DefaultRequestHeaders.Clear();
        client.DefaultRequestHeaders.Add("api-key", apiKey);

        HttpResponseMessage response = await client.PostAsync(openAiEndpoint, content);

        if (!response.IsSuccessStatusCode)
        {
            return $"Errore API: {response.StatusCode}";
        }

        var responseString = await response.Content.ReadAsStringAsync();
        var jsonResponse = JsonSerializer.Deserialize<JsonElement>(responseString);

        if (jsonResponse.TryGetProperty("choices", out JsonElement choices) &&
            choices[0].TryGetProperty("message", out JsonElement message) &&
            message.TryGetProperty("content", out JsonElement contentElement))
        {
            return contentElement.GetString().Trim();
        }

        return "Errore nella risposta dell'API.";
    }
}
