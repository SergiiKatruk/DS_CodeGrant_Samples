using DocuSign.eSign.Api;
using DocuSign.eSign.Client;
using DocuSign.eSign.Model;
using DocuSign.eSignature;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System;
using static DocuSign.eSign.Api.EnvelopesApi;

namespace WebApplication3
{
    [Route("EnvelopsList")]
    public class EnvelopsListModel : Controller
    {
        protected DSConfiguration Config { get; }
        protected IRequestItemsService RequestItemsService { get; }

        public EnvelopesInformation EnvelopesInformation { get; set; }

        public EnvelopsListModel(DSConfiguration config, IRequestItemsService requestItemsService)
        {
            Config = config;
            RequestItemsService = requestItemsService;
            ViewBag.title = "Envelops info";
        }

        [HttpGet]
        public IActionResult OnGet()
        {
            if (!CheckToken())
            {
                return Challenge();
            }
            string accessToken = RequestItemsService.User.AccessToken;
            string basePath = RequestItemsService.Session.BasePath + "/restapi";
            string accountId = RequestItemsService.Session.AccountId;
            var config = new Configuration(new ApiClient(basePath));
            config.AddDefaultHeader("Authorization", "Bearer " + accessToken);
            EnvelopesApi envelopesApi = new EnvelopesApi(config);
            ListStatusChangesOptions options = new ListStatusChangesOptions
            {
                fromDate = DateTime.Now.AddDays(-30).ToString("yyyy/MM/dd")
            };
            EnvelopesInformation = envelopesApi.ListStatusChanges(accountId, options);
            ViewBag.EnvelopesInformation = EnvelopesInformation;
            return View("EnvelopsList", this);
        }

        protected bool CheckToken(int bufferMin = 60)
        {
            return HttpContext.User.Identity.IsAuthenticated 
                && (DateTime.Now.Subtract(TimeSpan.FromMinutes(bufferMin)) <
                RequestItemsService.User.ExpireIn.Value);
        }
    }
}