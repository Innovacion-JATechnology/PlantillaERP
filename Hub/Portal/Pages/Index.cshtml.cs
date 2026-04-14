using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Portal.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly IConfiguration _configuration;

    public string? HelpDeskUrl { get; set; }
    public string? SaraERPUrl { get; set; }

    public IndexModel(ILogger<IndexModel> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }

    public void OnGet()
    {
        HelpDeskUrl = _configuration["ApplicationUrls:HelpDesk"];
        SaraERPUrl = _configuration["ApplicationUrls:SaraERP"];
    }
}
