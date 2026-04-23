namespace ApiSimexCsharp.DTO
{
    public class ClientListDTO
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string Industria { get; set; }
        public int EnviosActivosCount { get; set; }
        public string UltimaCargaResumen { get; set; }
        public int? TipusTransportId { get; set; }
    }
}

