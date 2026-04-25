using System.Text.Json.Serialization;

namespace ApiSimexCsharp.DTO
{
    public class TrackingStepDTO
    {
        public int Id { get; set; }
        public string Titol { get; set; } = string.Empty;

        [JsonPropertyName("data_hora")]
        public string DataHora { get; set; } = string.Empty;

        [JsonPropertyName("te_document")]
        public bool TeDocument { get; set; }

        [JsonPropertyName("nom_fitxer")]
        public string? NomFitxer { get; set; }

        public string Comentari { get; set; } = string.Empty;

        [JsonPropertyName("estaCompletado")]
        public int EstaCompletado { get; set; }
    }
}
