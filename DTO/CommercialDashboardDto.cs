namespace ApiSimexCsharp.DTO
{
    public class CommercialDashboardDto
    {
        // Usamos nombres en minúscula para que coincidan con el JSON de Android
        public string user_name { get; set; }
        public int pending_count { get; set; }
        public int active_ops_count { get; set; }
        public int rejected_count { get; set; }
    }
}
