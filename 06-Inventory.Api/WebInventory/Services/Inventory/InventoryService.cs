using Microsoft.Extensions.Options;
using System.Security.AccessControl;
using System.Text.Json;
using WebInventory.Infraestructure.Inventory;
using WebInventory.Services.DTO;
using WebInventory.ViewModels;

namespace WebInventory.Services.Inventory
{
    public class InventoryService : IInventoryService
    {
        private readonly HttpClient _httpClient;
        private readonly IOptions<SettingsValue> _settings;
        private readonly string _remoteServiceBaseUrl;

        JsonSerializerOptions options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };

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

            var httpResponse = await _httpClient.DeleteAsync(uri);
        }



        public async Task<string> GetAllItems()
        {
            var uri = Infraestructure.Inventory.API.Items.GetAllItems(_remoteServiceBaseUrl);
            var httpResponse = await _httpClient.GetAsync(uri);

            return await httpResponse.Content.ReadAsStringAsync();
        }

        public async Task<Item> GetItem(int code)
        {
            var uri = Infraestructure.Inventory.API.Items.GetItem(_remoteServiceBaseUrl, code);

            var response = await _httpClient.GetAsync(uri);
            var responseString = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var data = JsonSerializer.Deserialize<Item>(responseString, options);
                return data;
            }
            else
            {
                return null;
            }
        }

        public async Task CreateItem(ItemsDTO item)
        {
            var uri = Infraestructure.Inventory.API.Items.CreateItem(_remoteServiceBaseUrl);
            var httpResponse = await _httpClient.PostAsJsonAsync(uri, item);
        }

        public async Task SaveItem(ItemsDTO item)
        {
            var uri = Infraestructure.Inventory.API.Items.SaveItem(_remoteServiceBaseUrl);
            var httpResponse = await _httpClient.PutAsJsonAsync(uri, item);
        }

        public async Task<MessageResponseDTO> DeleteItem(int code)
        {
            MessageResponseDTO response = new MessageResponseDTO();
            var uri = Infraestructure.Inventory.API.Items.DeleteItem(_remoteServiceBaseUrl, code);

            var httpResponse = await _httpClient.DeleteAsync(uri);

            if (!httpResponse.IsSuccessStatusCode)
            {
                var responseContent = await httpResponse.Content.ReadAsStringAsync();
                response.Message = responseContent;
                response.Type = "Danger";

                return response;
            }
            else
            {
                response = JsonSerializer.Deserialize<MessageResponseDTO>(await httpResponse.Content.ReadAsStringAsync(), options);

                return response;
            }

        }
    }


}
