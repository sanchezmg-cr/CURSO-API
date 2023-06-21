using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.VisualBasic;
using WebInventory.Services.Inventory;

namespace WebInventory.Pages.Category
{
    public class IndexModel : PageModel
    {
        private readonly IInventoryService _inventoryService;
        public IHtmlLocalizer<Resources.SharedLabels> _sharedLabels{ get; set; }

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
