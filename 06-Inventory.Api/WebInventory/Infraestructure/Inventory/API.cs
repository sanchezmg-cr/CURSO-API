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

            public static string CreateCategory(string baseUri)
            {
                return $"{baseUri}Categories/CreateCategory";
            }

            public static string SaveCategory(string baseUri)
            {
                return $"{baseUri}Categories/SaveCategory";
            }

            public static string DeleteCategory(string baseUri, int categoryId)
            {
                return $"{baseUri}Categories/DeleteCategory/{categoryId}";
            }
        }

        public static class Items
        {
            public static string GetAllItems(string baseUri)
            {
                return $"{baseUri}Items/GetAllItems";
            }

            public static string GetItem(string baseUri, int code)
            {
                return $"{baseUri}Items/GetItem/{code}";
            }

            public static string CreateItem(string baseUri)
            {
                return $"{baseUri}Items/CreateItem";
            }

            public static string SaveItem(string baseUri)
            {
                return $"{baseUri}Items/SaveItem";
            }

            public static string DeleteItem(string baseUri, int code)
            {
                return $"{baseUri}Itemss/DeleteItem/{code}";
            }
        }
    }
}
