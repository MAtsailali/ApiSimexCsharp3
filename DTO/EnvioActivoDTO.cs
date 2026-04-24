using System.Text.Json.Serialization;

namespace ApiSimexCsharp.DTO
{
    public class EnvioActivoDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("cliente")]
        public string Cliente { get; set; } = string.Empty;

        [JsonPropertyName("ruta_origen")]
        public string RutaOrigen { get; set; } = string.Empty;

        [JsonPropertyName("ruta_destino")]
        public string RutaDestino { get; set; } = string.Empty;

        [JsonPropertyName("concepto")]
        public string Concepto { get; set; } = string.Empty;

        [JsonPropertyName("estado")]
        public string Estado { get; set; } = string.Empty;

        [JsonPropertyName("estado_id")]
        public int EstadoId { get; set; }

        [JsonPropertyName("fecha_creacion")]
        public string FechaCreacion { get; set; } = string.Empty;

        [JsonPropertyName("transport_type_id")]
        public int TransportTypeId { get; set; }

        [JsonPropertyName("precio")]
        public string Precio { get; set; } = "0.00";
    }
}
