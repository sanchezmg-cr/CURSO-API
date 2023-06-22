using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.VisualBasic;
using System.Text.Json;
using WebInventory.Services.DTO;
using WebInventory.Services.Inventory;

namespace WebInventory.Pages.Category
{
    public class IndexModel : PageModel
    {
        private readonly IInventoryService _inventoryService;
        public IHtmlLocalizer<Resources.SharedLabels> _sharedLabels { get; set; }

        public IndexModel(IInventoryService inventoryService, IHtmlLocalizer<Resources.SharedLabels> sharedLabels)
        {
            _inventoryService = inventoryService;
            _sharedLabels = sharedLabels;
        }

        // accion a ejecutar insert, update
        [BindProperty]
        public string ActionType { get; set; }

        // los datos que vienen en json
        [BindProperty]
        public string ActionData { get; set; }

        public string DataSource { get; set; }

        JsonSerializerOptions options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };

        public async Task<ActionResult> OnPost()
        {
            CategoryDTO category;
            if (!string.IsNullOrEmpty(ActionType) && !string.IsNullOrEmpty(ActionData))
            {
                switch (ActionType)
                {
                    case "Create":
                        category = JsonSerializer.Deserialize<CategoryDTO>(ActionData, options);
                        await _inventoryService.CreateCategory(category);
                        break;

                    case "Edit":
                        category = JsonSerializer.Deserialize<CategoryDTO>(ActionData, options);
                        await _inventoryService.SaveCategory(category);
                        break;

                    case "Delete":
                        //category = JsonSerializer.Deserialize<CategoryDTO>(ActionData, options);
                        await _inventoryService.DeleteCategory(int.Parse(ActionData));
                        break;
                }
            }

            await GetRequiredDataLoadPage();
            return RedirectToPage("./Index");
        }

        public async Task<ActionResult> OnGet()
        {
            await GetRequiredDataLoadPage();
            return Page();
        }

        private async Task GetRequiredDataLoadPage()
        {
            var response = await _inventoryService.GetAllCategories();

            DataSource = response;
        }
    }
}
