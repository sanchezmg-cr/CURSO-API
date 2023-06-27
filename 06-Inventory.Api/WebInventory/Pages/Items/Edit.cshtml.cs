using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using System.Reflection.Metadata.Ecma335;
using System.Text.Encodings.Web;
using System.Text.Json;
using WebInventory.Helpers;
using WebInventory.Services.DTO;
using WebInventory.Services.Inventory;

namespace WebInventory.Pages.Items
{
    public class EditModel : PageModel
    {
        public IHtmlLocalizer<Resources.SharedLabels> SharedLabelsLocalizer { get; private set; }
        public IHtmlLocalizer<Resources.SharedMessages> SharedMessagesLocalizer { get; private set; }
        public IHtmlLocalizer<Resources.SharedValidationMessages> SharedValidationMessagesLocalizer { get; private set; }
        public IHtmlLocalizer<EditModel> Localizer { get; private set; }
        private readonly IOptions<SettingsValue> _settings;
        private readonly IInventoryService _inventoryService;

        public string DataSource { get; set; }
        JsonSerializerOptions options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };

        [BindProperty]
        public ViewModels.Item Item { get; set; }

        public EditModel(IHtmlLocalizer<Resources.SharedLabels> sharedLabelsLocalizer,
                           IHtmlLocalizer<Resources.SharedMessages> sharedMessagesLocalizer,
                           IHtmlLocalizer<Resources.SharedValidationMessages> sharedValidationMessagesLocalizer,
                           IHtmlLocalizer<EditModel> localizer,
                           IOptions<SettingsValue> settings,
                           IInventoryService inventoryService)
        {
            SharedLabelsLocalizer = sharedLabelsLocalizer;
            SharedMessagesLocalizer = sharedMessagesLocalizer;
            SharedValidationMessagesLocalizer = sharedValidationMessagesLocalizer;
            Localizer = localizer;
            _settings = settings;
            _inventoryService = inventoryService;
        }

        public async Task<IActionResult> OnGetAsync(string code)
        {
            if (string.IsNullOrEmpty(code) || code.Equals(nameof(code)))
                return RedirectToPage("./Index");

            await GetRequiredDataToLoadPage(code);
            return Page();

        }

        private async Task GetRequiredDataToLoadPage(string code)
        {
            PageMessage msg;
            //ViewModels.Item item = new ViewModels.Item();
            try
            {
                var response = _inventoryService.GetItem(Convert.ToInt32(code));

                await Task.WhenAll(response);


                Item = response.Result;
            }
            catch (Exception ex)
            {
                msg = new PageMessage(MessageType.Danger, SharedMessagesLocalizer.GetString("LoadingDataError", HtmlEncoder.Default.Encode(ex.Message)));
                TempData["PageMessage"] = JsonSerializer.Serialize(msg);
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            PageMessage msg = null;
            try
            {
                ItemsDTO item = new ItemsDTO();


                item = new ItemsDTO()
                {
                    Code = Item.Code,
                    Description = Item.Description,
                    Category = Item.Category,
                    Brand = Item.Brand,
                    Weight = Item.Weight,
                    BarCode = Item.BarCode,
                    UpdateDate = Item.UpdateDate
                };

                await _inventoryService.SaveItem(item);

                msg = new PageMessage(MessageType.Success, SharedMessagesLocalizer.GetString("EditSuccessMessage"));
                TempData["PageMessage"] = JsonSerializer.Serialize(msg);

                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                msg = new PageMessage(MessageType.Danger, SharedMessagesLocalizer.GetString("CreateErrorMessage", HtmlEncoder.Default.Encode(ex.Message)));
                TempData["PageMessage"] = JsonSerializer.Serialize(msg);
                return Page();
            }

            return RedirectToPage("./Index");
        }

        public string GetJSONViewModel()
        {
            //return JsonConvert.SerializeObject(Item);
            return JsonSerializer.Serialize(Item, options);
        }
    }
}
