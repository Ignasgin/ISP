using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ISP.Pages
{
    public class Reserve : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public string Make { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Price { get; set; }

        [BindProperty, DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        [BindProperty, DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }

        [BindProperty]
        public bool ShortTime { get; set; }

        public void OnGet()
        {
            // Properties are automatically set from the query parameters due to SupportsGet = true
        }
    }
}
