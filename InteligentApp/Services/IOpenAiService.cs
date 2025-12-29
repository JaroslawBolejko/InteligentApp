using InteligentApp.Components.Models;
using System.Net.Http;

namespace InteligentApp.Services
{
    public interface IOpenAiService
    {
        Task<string> GenerateTextAsync(string userPrompt, bool includeSystemMessage = true);
    }

    public class OpenAiService : IOpenAiService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public OpenAiService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        /// <summary>
        /// Sends a prompt to OpenAI and returns the response.
        /// </summary>
        /// <param name="userPrompt">Prompt to send.</param>
        /// <param name="includeSystemMessage">Whether to include the default system message.</param>
        /// <returns>Response text from OpenAI.</returns>
        public async Task<string> GenerateTextAsync(string userPrompt, bool includeSystemMessage = true)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("OpenAI");

                var messages = new List<object>();
                
                if (includeSystemMessage)
                {
                    messages.Add(new { role = "system", content = "Jesteś pomocnym asystentem." });
                }
                
                messages.Add(new { role = "user", content = userPrompt });

                var requestBody = new
                {
                    model = "gpt-4",
                    messages = messages.ToArray(),
                    max_tokens = 500,
                    temperature = 0.7
                };

                using var response = await client.PostAsJsonAsync("", requestBody);

                if (!response.IsSuccessStatusCode)
                    return $"Error: {response.StatusCode}, Przepraszam ale nie udało mi się uzyskać odpowiedzi od AI";

                var jsonResponse = await response.Content.ReadFromJsonAsync<ChatCompletionResponse>();
                var answer = jsonResponse?.Choices?.FirstOrDefault()?.Message?.Content?.Trim();
                return answer ?? "Przepraszam ale nie udało mi się uzyskać odpowiedzi od AI";
            }
            catch (Exception ex)
            {
                return $"Wystąpił błąd: {ex.Message}";
            }
        }
    }
}
