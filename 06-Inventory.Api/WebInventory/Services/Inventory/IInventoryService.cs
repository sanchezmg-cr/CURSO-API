using WebInventory.Services.DTO;

namespace WebInventory.Services.Inventory
{
    public interface IInventoryService
    {
        #region Category
        Task<string> GetAllCategories();
        Task CreateCategory(CategoryDTO category);
        Task SaveCategory(CategoryDTO category);
        Task DeleteCategory(int categoryId);
        #endregion


        #region Articulos
        Task<string> GetAllItems();
        #endregion

    }
}
