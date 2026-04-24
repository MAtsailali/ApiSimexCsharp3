namespace ApiSimexCsharp.DTO
{
    using System.Text.Json.Serialization;

    namespace ApiSimexCsharp.DTO
    {
        public class DetalleEnvioDto
        {
            [JsonPropertyName("id")]
            public int Id { get; set; }

            [JsonPropertyName("order_number")]
            public string OrderNumber => $"#SHP-{Id}";

            [JsonPropertyName("cliente")]
            public string Cliente { get; set; } = string.Empty;

            [JsonPropertyName("ruta_completa")]
            public string RutaCompleta { get; set; } = string.Empty;

            [JsonPropertyName("concepto")]
            public string Concepto { get; set; } = string.Empty;

            [JsonPropertyName("estado_actual")]
            public string EstadoActual { get; set; } = string.Empty;

            [JsonPropertyName("fecha_creacion")]
            public string FechaCreacion { get; set; } = string.Empty;

            // Lista de pasos para el seguimiento vertical
            [JsonPropertyName("tracking_steps")]
            public List<TrackingStepDto> TrackingSteps { get; set; } = new();
        }

        public class TrackingStepDto
        {
            [JsonPropertyName("id")]
            public int Id { get; set; }

            [JsonPropertyName("titol")]
            public string Titol { get; set; } = string.Empty;

            [JsonPropertyName("data_hora")]
            public string DataHora { get; set; } = string.Empty;

            [JsonPropertyName("te_document")]
            public bool TeDocument { get; set; }

            [JsonPropertyName("nom_fitxer")]
            public string? NomFitxer { get; set; }

            [JsonPropertyName("comentari")]
            public string Comentari { get; set; } = string.Empty;
        }
    }
}
