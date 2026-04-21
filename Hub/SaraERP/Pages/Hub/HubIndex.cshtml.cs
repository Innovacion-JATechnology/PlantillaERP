using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;     
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SaraERP.Pages.Hub
{
    [Authorize] 
    public class HubIndexModel : PageModel
    {

        private readonly IConfiguration _configuration;

        public string SaraERPUrl { get; private set; }

        public string HelpDeskUrl { get; private set; }

        public HubIndexModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void OnGet()
        {
            SaraERPUrl = _configuration["ApplicationUrls:SaraERP"];
            HelpDeskUrl = _configuration["ApplicationUrls:HelpDesk"];
        }

    }
}
