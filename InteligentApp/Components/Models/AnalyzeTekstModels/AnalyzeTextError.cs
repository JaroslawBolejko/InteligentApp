using System.Text.Json.Serialization;

namespace InteligentApp.Components.Models.AnalyzeTekstModels
{
    public class AnalyzeTextError
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("error")]
        public ErrorDetails Error { get; set; }
    }
}