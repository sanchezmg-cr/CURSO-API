using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using WebInventory.Helpers;
using System.Text.Encodings.Web;
using System.Text.Json;
using WebInventory.Services.Inventory;

namespace WebInventory.ViewComponents
{
    public class ItemDetailsViewComponent : ViewComponent
    {
        private readonly IInventoryService _inventoryService;
        public IHtmlLocalizer<Resources.SharedMessages> SharedMessagesLocalizer { get; private set; }

        public ItemDetailsViewComponent(IHtmlLocalizer<Resources.SharedMessages> sharedMessagesLocalizer,
                                                   IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
            SharedMessagesLocalizer = sharedMessagesLocalizer;
        }

        public async Task<IViewComponentResult> InvokeAsync(string code)
        {
            PageMessage msg;
            ViewModels.Item item = new ViewModels.Item();
            try
            {
                var response = _inventoryService.GetItem(Convert.ToInt32(code));

                await Task.WhenAll(response);


                item = response.Result;
            }
            catch (Exception ex)
            {
                msg = new PageMessage(MessageType.Danger, SharedMessagesLocalizer.GetString("LoadingDataError", HtmlEncoder.Default.Encode(ex.Message)));
                TempData["PageMessage"] = JsonSerializer.Serialize(msg);
            }
            return View(item);
        }
    }
}
