using WebInventory.Services.DTO;
using WebInventory.ViewModels;

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
        Task<Item> GetItem(int code);
        Task CreateItem(ItemsDTO category);
        Task SaveItem(ItemsDTO category);

        Task<MessageResponseDTO> DeleteItem(int code);
        #endregion
    }
}
