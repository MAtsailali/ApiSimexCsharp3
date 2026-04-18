using System.Reflection.PortableExecutable;

namespace ApiSimexCsharp.DTO
{
    public class LoginDTO
    {
        public int Id { get; set; }
        public string correu { get; set; }
        public string contrasenya { get; set; }
        public string nom {  get; set; }
        public string cognoms { get; set; }
        public int rolId { get; set; }
        public int? companyId { get; set; }
        public int? status { get; set; }
        public string? tlfn { get; set; }
        public System.DateTime? ultima_conex { get;set; }
        public string Token { get; set; }
    }
}
