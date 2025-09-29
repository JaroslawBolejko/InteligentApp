using System.Text.Json.Serialization;

namespace InteligentApp.Components.Models.AnalyzeTekstModels
{
    public class AnalyzeTextResponse
    {
        [JsonPropertyName("kind")]
        public string Kind { get; set; }

        [JsonPropertyName("results")]
        public AnalyzeTextResults Results { get; set; }
    }
}
