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
        private readonly IHtmlLocalizer<Resources.SharedMessages> _sharedMessagesLocalizer;
        private readonly IHtmlLocalizer<Resources.SharedValidationMessages> _sharedValidationMessagesLocalizer;
        public IHtmlLocalizer<IndexModel> Localizer { get; private set; }
        private readonly IOptions<SettingsValue> _settings;

        public IHtmlLocalizer<Resources.SharedLabels> SharedLabelsLocalizer { get; private set; }
        private readonly IInventoryService _inventoryService;

        public IndexModel(
                          IHtmlLocalizer<Resources.SharedMessages> sharedMessagesLocalizer,
                          IHtmlLocalizer<Resources.SharedValidationMessages> sharedValidationMessagesLocalizer,
                          IHtmlLocalizer<IndexModel> localizer,
                          IOptions<SettingsValue> settings,
                          IHtmlLocalizer<Resources.SharedLabels> sharedLabelsLocalizer,
                          IInventoryService inventoryService
                          )
        {
            _sharedMessagesLocalizer = sharedMessagesLocalizer;
            _sharedValidationMessagesLocalizer = sharedValidationMessagesLocalizer;
            Localizer = localizer;
            _settings = settings;

            _inventoryService = inventoryService;
            SharedLabelsLocalizer = sharedLabelsLocalizer;
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
            var response = await _inventoryService.GetAllItems();

            DataSource = response;
        }
    }
}
