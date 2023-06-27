using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using System.Text.Encodings.Web;
using WebInventory.Helpers;
using WebInventory.Services.Inventory;

namespace WebInventory.Pages.Items
{
    public class DeleteModel : PageModel
    {

        public IHtmlLocalizer<Resources.SharedLabels> SharedLabelsLocalizer { get; private set; }
        public IHtmlLocalizer<Resources.SharedMessages> SharedMessagesLocalizer { get; private set; }
        private readonly IOptions<SettingsValue> _settings;
        private readonly IInventoryService _inventoryService;

        [BindProperty]
        public string Code { get; set; }


        public DeleteModel(IHtmlLocalizer<Resources.SharedLabels> sharedLabelsLocalizer,
                        IOptions<SettingsValue> settings,
                        IInventoryService inventoryService,
                        IHtmlLocalizer<Resources.SharedMessages> sharedMessagesLocalizer)
        {
            SharedLabelsLocalizer = sharedLabelsLocalizer;
            SharedMessagesLocalizer = sharedMessagesLocalizer;
            _settings = settings;
            _inventoryService = inventoryService;
        }

        public ActionResult OnGet(string code)
        {
            if (string.IsNullOrEmpty(code) || code.Equals(nameof(code)))
                return RedirectToPage("./Index");

            Code = code;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            PageMessage msg;
            try
            {
                if (string.IsNullOrEmpty(Code))
                {
                    msg = new PageMessage(MessageType.Danger, SharedMessagesLocalizer.GetString("NotFoundDeleteMessage"));
                    //TempData["PageMessage"] = JsonConvert.SerializeObject(msg);
                }
                else
                {
                    var response = await _inventoryService.DeleteItem(Convert.ToInt32(Code));

                    if (response.Type.Equals("Success"))
                    {

                        msg = new PageMessage(MessageType.Success, response.Message);

                        //TempData["PageMessage"] = JsonConvert.SerializeObject(msg);

                    }
                    else
                    {

                        //var resultMessage = JsonConvert.SerializeObject(new PageMessage(MessageType.Danger, response.Message));

                        //return new JsonResult(resultMessage);

                    };

                }

                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                msg = new PageMessage(MessageType.Danger, SharedMessagesLocalizer.GetString("DeleteErrorMessage", HtmlEncoder.Default.Encode(ex.Message)));
                //TempData["PageMessage"] = JsonConvert.SerializeObject(msg);
            }
            return Page();
        }
    }
}
