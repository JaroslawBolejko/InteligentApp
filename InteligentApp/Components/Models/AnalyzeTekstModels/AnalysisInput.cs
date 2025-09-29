using System.Text.Json.Serialization;

namespace InteligentApp.Components.Models.AnalyzeTekstModels
{
    public class AnalysisInput
    {
        [JsonPropertyName("documents")]
        public List<AnalysisDocument> Documents { get; set; }
    }
}
