using Microsoft.Extensions.Options;
using WebInventory.Infraestructure.Inventory;
using WebInventory.Services.DTO;

namespace WebInventory.Services.Inventory
{
    public class InventoryService : IInventoryService
    {
        private readonly HttpClient _httpClient;
        private readonly IOptions<SettingsValue> _settings;
        private readonly string _remoteServiceBaseUrl;

        public InventoryService(HttpClient httpClient, IOptions<SettingsValue> settings)
        {
            _httpClient = httpClient;
            _settings = settings;
            _remoteServiceBaseUrl = $"{settings.Value.InventoryAPIEndpoint}";
        }
        public async Task<string> GetAllCategories()
        {
            var uri = Infraestructure.Inventory.API.Categoria.GetAllCategories(_remoteServiceBaseUrl);
            var httpResponse = await _httpClient.GetAsync(uri);

            return await httpResponse.Content.ReadAsStringAsync();
        }

        public async Task CreateCategory(CategoryDTO category)
        {
            var uri = Infraestructure.Inventory.API.Categoria.CreateCategory(_remoteServiceBaseUrl);
            var httpResponse = await _httpClient.PostAsJsonAsync(uri, category);
        }

        public async Task SaveCategory(CategoryDTO category)
        {
            var uri = Infraestructure.Inventory.API.Categoria.SaveCategory(_remoteServiceBaseUrl);
            var httpResponse = await _httpClient.PutAsJsonAsync(uri, category);
        }

        public async Task DeleteCategory(int categoryId)
        {
            var uri = Infraestructure.Inventory.API.Categoria.DeleteCategory(_remoteServiceBaseUrl, categoryId);

            var httpResponse = await _httpClient.PostAsync(uri, null);
        }

        public async Task<string> GetAllItems()
        {
            var uri = Infraestructure.Inventory.API.Items.GetAllItems(_remoteServiceBaseUrl);
            var httpResponse = await _httpClient.GetAsync(uri);

            return await httpResponse.Content.ReadAsStringAsync();
        }
    }
}
