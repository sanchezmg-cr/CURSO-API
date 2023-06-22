using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
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

        public IHtmlLocalizer<Resources.SharedLabels> SharedLabelsLocalizer { get; private set; }
        public string DataSource { get; set; }

        public IndexModel(IHtmlLocalizer<Resources.SharedMessages> sharedMessagesLocalizer,
                          IHtmlLocalizer<Resources.SharedValidationMessages> sharedValidationMessagesLocalizer,
                          IHtmlLocalizer<Resources.SharedLabels> sharedLabelsLocalizer,
                          IHtmlLocalizer<IndexModel> localizer,
                          IOptions<SettingsValue> settings,
                          IInventoryService inventoryService)
        {
            _sharedValidationMessagesLocalizer = sharedValidationMessagesLocalizer;
            _sharedMessagesLocalizer = sharedMessagesLocalizer;
            Localizer = localizer;
            _settings = settings;
            _inventoryService = inventoryService;
            SharedLabelsLocalizer = sharedLabelsLocalizer;
        }

        public void OnGet()
        {
            GetRequiredDataLoadPage();
        }

        private async Task GetRequiredDataLoadPage()
        {
            var response = await _inventoryService.GetAllItems();

            DataSource = response;
        }
    }
}
