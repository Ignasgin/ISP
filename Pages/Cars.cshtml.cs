using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ISP.Pages
{
    public class CarsModel : PageModel
    {
        private readonly ILogger<CarsModel> _logger;

        public CarsModel(ILogger<CarsModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}