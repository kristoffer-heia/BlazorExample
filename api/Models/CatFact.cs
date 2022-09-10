using System.Text.Json.Serialization;

namespace api.Cat
{
    public class CatFact
    {
        [JsonPropertyName("fact")]
        public string? Fact { get; set; }

        [JsonPropertyName("length")]
        public int Length { get; set; }
    }
}
