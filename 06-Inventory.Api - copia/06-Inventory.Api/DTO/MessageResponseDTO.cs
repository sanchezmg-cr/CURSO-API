namespace _06_Inventory.Api.DTO
{
    public class MessageResponseDTO
    {
        public int Code { get; set; }
        public string Type { get; set; }
        public string Message { get; set; }
        public DateTime? Date { get; set; }
        public string Data { get; set; }
    }
}
