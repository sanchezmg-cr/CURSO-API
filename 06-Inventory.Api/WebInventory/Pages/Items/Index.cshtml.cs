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

        private readonly IInventoryService _inventoryService;
        public IHtmlLocalizer<Resources.SharedLabels> SharedLabelsLocalizer { get; set; }


        public IndexModel(IHtmlLocalizer<Resources.SharedMessages> sharedMessagesLocalizer,
                          IHtmlLocalizer<Resources.SharedValidationMessages> sharedValidationMessagesLocalizer,
                          IHtmlLocalizer<IndexModel> localizer,
                          IInventoryService inventoryService,
                          IOptions<SettingsValue> settings,
                          IHtmlLocalizer<Resources.SharedLabels> sharedLabelsLocalizer)
        {
            _sharedMessagesLocalizer = sharedMessagesLocalizer;
            _sharedValidationMessagesLocalizer = sharedValidationMessagesLocalizer;
            Localizer = localizer;
            _settings = settings;
            _inventoryService = inventoryService;
            SharedLabelsLocalizer = sharedLabelsLocalizer;
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
