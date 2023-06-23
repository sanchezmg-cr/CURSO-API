using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic;
using System.Text.Json;
using WebInventory.Services.DTO;
using WebInventory.Services.Inventory;

namespace WebInventory.Pages.Items
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

        public string DataSource { get; set; }

        JsonSerializerOptions options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };

        public async Task<ActionResult> OnGet()
        {
            await GetRequiredDataLoadPage();
            return Page();
        }

        private async Task GetRequiredDataLoadPage()
        {
            var response = await _inventoryService.GetAllItems();

            DataSource = response;
        }
    }
}
