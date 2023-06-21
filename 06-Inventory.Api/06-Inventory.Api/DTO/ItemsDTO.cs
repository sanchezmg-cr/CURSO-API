namespace _06_Inventory.Api.DTO
{
    public class ItemsDTO
    {
        public long Code { get; set; }
        public string Description { get; set; }

        public int Category { get; set; }
        public string CategoryDescription { get; set; }

        public string Brand { get; set; }
        public decimal Weight { get; set; }
        public string BarCode { get; set; }

        public DateTime? CreateDate { get; set; }
        public string CreateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string UpdateUser { get; set; }
    }
}
