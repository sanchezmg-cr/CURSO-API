namespace WebInventory.Infraestructure.Inventory
{
    public class API
    {
        public static class Categoria
            {
            public static string GetAllCategories(string baseUri)
        {
                return $"{baseUri}Categories/GetAllCategories";
        }
    }
}
}
