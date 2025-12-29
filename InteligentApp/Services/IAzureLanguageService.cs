using InteligentApp.Components.Models.AnalyzeTekstModels;
using System.Text.Json;

namespace InteligentApp.Services
{
    public interface IAzureLanguageService
    {
        Task<AnalyzeTextDocument> AnalyzeTextApiAsync(string userInput, string kind);
    }

    public class AzureLanguageService : IAzureLanguageService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AzureLanguageService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        /// <summary>
        /// Calls Azure AI API for the specified analysis kind.
        /// </summary>
        /// <param name="userInput">Text to analyze.</param>
        /// <param name="kind">Analysis kind ("SentimentAnalysis", "KeyPhraseExtraction", etc.).</param>
        /// <returns>AnalyzeTextDocument with results.</returns>
        public async Task<AnalyzeTextDocument> AnalyzeTextApiAsync(string userInput, string kind)
        {
            try
            {
                var requestBody = BuildAnalyzeTextRequest(userInput, kind);
                var client = _httpClientFactory.CreateClient("AzureAI");
                var endpoint = "language/:analyze-text?api-version=2024-11-01";

                var response = await client.PostAsJsonAsync(endpoint, requestBody);
                response.EnsureSuccessStatusCode();

                var responseResult = await response.Content.ReadAsStringAsync();
                var analysisResponse = JsonSerializer.Deserialize<AnalyzeTextResponse>(responseResult);
                return analysisResponse?.Results?.Documents?.FirstOrDefault() ?? new AnalyzeTextDocument();
            }
            catch (Exception ex)
            {
                // Return error info in a document for display
                if (kind == "SentimentAnalysis")
                    return new AnalyzeTextDocument { Sentiment = $"Błąd podczas analizy sentymentu: {ex.Message}" };
                if (kind == "KeyPhraseExtraction")
                    return new AnalyzeTextDocument { KeyPhrases = new List<string> { $"Wystąpił błąd podczas ekstrakcji kluczowych fraz: {ex.Message}" } };
                return new AnalyzeTextDocument();
            }
        }

        /// <summary>
        /// Builds the request object for Azure AI text analysis.
        /// </summary>
        /// <param name="userInput">Text to analyze.</param>
        /// <param name="kind">Analysis kind.</param>
        /// <returns>AnalyzeTextRequest object.</returns>
        private AnalyzeTextRequest BuildAnalyzeTextRequest(string userInput, string kind)
        {
            return new AnalyzeTextRequest
            {
                Kind = kind,
                AnalysisInput = new AnalysisInput
                {
                    Documents = new List<AnalysisDocument>
                {
                    new AnalysisDocument { Id = "1", Language = "pl", Text = userInput }
                }
                },
                Parameters = new Dictionary<string, object>
            {
                { "modelVersion", "latest" },
                { "loggingOptOut", false }
            }
            };
        }
    }
}
