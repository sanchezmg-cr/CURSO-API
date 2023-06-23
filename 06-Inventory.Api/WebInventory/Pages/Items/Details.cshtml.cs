using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;

namespace WebInventory.Pages.Items
{
    public class DetailsModel : PageModel
    {

        public IHtmlLocalizer<Resources.SharedLabels> SharedLabelsLocalizer { get; private set; }
        public IHtmlLocalizer<Resources.SharedMessages> SharedMessagesLocalizer { get; private set; }
        private readonly IOptions<SettingsValue> _settings;
        [BindProperty]
        public string CodeId { get; set; }
        [BindProperty]
        public string FormId { get; set; }

        public DetailsModel(IHtmlLocalizer<Resources.SharedLabels> sharedLabelsLocalizer,
                        IOptions<SettingsValue> settings,
                        IHtmlLocalizer<Resources.SharedMessages> sharedMessagesLocalizer)
        {
            SharedLabelsLocalizer = sharedLabelsLocalizer;
            SharedMessagesLocalizer = sharedMessagesLocalizer;
            _settings = settings;
        }

        public ActionResult OnGet(string form, string codeId)
        {
            //PageMessage msg;
            //if (!base.ValidateFunctionalityAccessPolicy(SecurityConstants.FunctionalitiesConstants.General.Maintenances.MT_FORMS, SecurityConstants.ActionOptions.View))
            //{
            //    msg = new PageMessage(MessageType.Danger, SharedMessagesLocalizer.GetString("NoAccessFunctionality"));
            //    TempData["PageMessage"] = JsonConvert.SerializeObject(msg);

            //    return RedirectToPage("/_Error");
            //}

            if (string.IsNullOrEmpty(codeId) || string.IsNullOrEmpty(form) || form.Equals(nameof(form)) || codeId.Equals(nameof(codeId)))
                return RedirectToPage("./Index");

            CodeId = codeId;
            FormId = form;
            return Page();
        }

    }
}
