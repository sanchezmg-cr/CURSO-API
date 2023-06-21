using Microsoft.Extensions.Options;
using WebInventory.Infraestructure.Inventory;

namespace WebInventory.Services.Inventory
{
    public class InventoryService : IInventoryService
    {
        private readonly HttpClient _httpClient;
        private readonly IOptions<SettingsValue> _settings;
        private readonly string _remoteServiceBaseUrl;

        public InventoryService(HttpClient httpClient, IOptions<SettingsValue> settings )
        {
            _httpClient = httpClient;
            _settings = settings;
            _remoteServiceBaseUrl = $"{settings.Value.InventoryAPIEndpoint}";            
        }
        public async Task<string> GetAllCategories()
        {
            var uri = Infraestructure.Inventory.API.Categoria.GetAllCategories(_remoteServiceBaseUrl);
            var httpResponse = await _httpClient.GetAsync( uri );

            return await httpResponse.Content.ReadAsStringAsync();
        }
    }
}
