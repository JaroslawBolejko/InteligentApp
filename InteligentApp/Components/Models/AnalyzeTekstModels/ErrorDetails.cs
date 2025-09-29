using System.Text.Json.Serialization;

namespace InteligentApp.Components.Models.AnalyzeTekstModels
{
    public class ErrorDetails
    {
        [JsonPropertyName("code")]
        public string Code { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }
    }
}