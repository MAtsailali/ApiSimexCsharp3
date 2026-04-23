namespace ApiSimexCsharp.DTO
{
    public class UserProfileDto
    {
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string RoleName { get; set; }

        // Datos de la Empresa
        public int? CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string IndustryName { get; set; }
        public string ColaboradorId { get; set; }
    }
}
