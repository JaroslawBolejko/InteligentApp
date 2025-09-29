using System.Text.Json.Serialization;

namespace InteligentApp.Components.Models.AnalyzeTekstModels
{
    public class AnalyzeTextRequest
    {
        [JsonPropertyName("kind")]
        public string Kind { get; set; }

        [JsonPropertyName("analysisInput")]
        public AnalysisInput AnalysisInput { get; set; }

        [JsonPropertyName("parameters")]
        public Dictionary<string, object> Parameters { get; set; }
    }
}
