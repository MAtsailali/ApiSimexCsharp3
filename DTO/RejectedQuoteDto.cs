namespace ApiSimexCsharp.DTO
{
    public class RejectedQuoteDto
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string RejectionReason { get; set; }
        public string ImageUrl { get; set; }
        public string Route { get; set; }
    }
}
