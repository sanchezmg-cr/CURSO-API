namespace WebInventory.Services.DTO
{
    public class CategoryDTO
    {
        public int Code { get; set; }
        public string Description { get; set; }

        public DateTime? CreateDate { get; set; }
        public string CreateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string UpdateUser { get; set; }
    }
}
