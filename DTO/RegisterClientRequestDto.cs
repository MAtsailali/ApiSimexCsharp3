namespace ApiSimexCsharp.DTO
{
    public class RegisterClientRequestDto
    {
        // Datos de la Company
        public string CompanyName { get; set; } = null!;
        public string IndustryName { get; set; } = null!; // El nombre que viene del Spinner/AutoComplete
        public string TaxId { get; set; } = null!; // El CIF

        public string CurrencyId { get; set; } = null!;

        // Datos del Usuario (Persona de contacto)
        public string Correu { get; set; } = null!;
        public string Nom { get; set; } = null!;
        public string Cognoms { get; set; } = null!;
        public string Tlfn { get; set; } = null!;
        public string Contrasenya { get; set; } = "123456"; // Contraseña por defecto o enviada

        // Datos financieros (Si decides guardarlos en la tabla Company, añade los campos aquí)
       
    }
}
