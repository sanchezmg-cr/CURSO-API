namespace WebInventory.ViewModels
{
    public class Item
    {
        public long Code { get; set; }
        public string Description { get; set; }

        public int Category { get; set; }
        public string CategoryDescription { get; set; }

        public string Brand { get; set; }
        public decimal Weight { get; set; }
        public string BarCode { get; set; }
    }
}
