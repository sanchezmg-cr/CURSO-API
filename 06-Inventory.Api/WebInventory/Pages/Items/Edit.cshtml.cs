using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using System.Reflection.Metadata.Ecma335;
using System.Text.Encodings.Web;
using System.Text.Json;
using WebInventory.Helpers;
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

        [BindProperty]
        public string ActionData { get; set; }
        [BindProperty]
        public ViewModels.Item Item { get; set; }
        [BindProperty]
        public bool ActiveDataGroup { get; set; }

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
        JsonSerializerOptions options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };

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

        //    public async Task<IActionResult> OnPostAsync()
        //    {
        //        PageMessage msg = null;
        //        try
        //        {
        //            if (ModelState.IsValid)
        //            {
        //                if (!string.IsNullOrEmpty(ActionData))
        //                {
        //                    var details = JsonConvert.DeserializeObject<IEnumerable<ConsecutivePeriod>>(ActionData);
        //                    FormDTO form = new FormDTO();

        //                    var companyDetails = await _generalServices.GetCompanyById(_identityService.GetCompanyId(), _identityService.GetToken());

        //                    form = new FormDTO()
        //                    {
        //                        CompanyId = base._identityService.GetCompanyId(),
        //                        Form = Form.Form,
        //                        Module = Form.Module,
        //                        InitialNumber = Form.InitialNumber,
        //                        FinalNumber = Form.FinalNumber,
        //                        AuthorizationDate = Form.AuthorizationDate,
        //                        AuthorizationNumber = Form.AuthorizationNumber,
        //                        Caption = Form.Caption,
        //                        Next = Form.Next,
        //                        Reserved = Form.Reserved,
        //                        ResetIndicator = Form.ResetIndicator,
        //                        Serial = Form.Serial,
        //                        Active = ActiveDataGroup,
        //                        Description = this.Form.Description,
        //                        UpdateUser = base._identityService.GetUserId()
        //                    };

        //                    var lstDetails = from A in details
        //                                     select new ConsecutivePeriodDTO()
        //                                     {
        //                                         CompanyId = form.CompanyId,
        //                                         Form = form.Form,
        //                                         Next = A.Next,
        //                                         Period = A.Period,
        //                                         Year = A.Year,
        //                                         Month = A.Month,
        //                                         InitialNumber = A.InitialNumber,
        //                                         FinalNumber = A.FinalNumber,
        //                                         UpdateDate = DateTime.Now,
        //                                         UpdateUser = base._identityService.GetUserId()
        //                                     };

        //                    form.Details = lstDetails.ToList();
        //                    DataSource = JsonConvert.SerializeObject(details, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
        //                    await _generalServices.UpdateForm(form, _identityService.GetToken());

        //                    msg = new PageMessage(MessageType.Success, SharedMessagesLocalizer.GetString("EditSuccessMessage"));
        //                    TempData["PageMessage"] = JsonConvert.SerializeObject(msg);

        //                    return RedirectToPage("./Index");
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            msg = new PageMessage(MessageType.Danger, SharedMessagesLocalizer.GetString("CreateErrorMessage", HtmlEncoder.Default.Encode(ex.Message)));
        //            TempData["PageMessage"] = JsonConvert.SerializeObject(msg);
        //            await GetRequiredDataToLoadPage(this.Form.Form, base._identityService.GetCompanyId());
        //            return Page();
        //        }

        //        return RedirectToPage("./Index");
        //    }

        public string GetJSONViewModel()
        {
            //return JsonConvert.SerializeObject(Item);
            return JsonSerializer.Serialize(Item, options);
        }
    }
}
