namespace _06_Inventory.Api.DTO
{
    public class CategoriaDTO
    {
        public int Code { get; set; }
        public string Description { get; set; }

        public DateTime? CreateDate { get; set; }
        public string CreateUser { get; set; }
        public DateTime? updateDate { get; set; }
        public string updateUser { get; set; }
    }
}
