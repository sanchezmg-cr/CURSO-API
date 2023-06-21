namespace WebInventory.Infraestructure.Inventory
{
    public class API
    {
        public static class Categoria
            {
            public static string GetAllCategorias(string baseUri)
        {
                return $"{baseUri}Categoria/GetAllCategorias";
        }
    }
}
}
