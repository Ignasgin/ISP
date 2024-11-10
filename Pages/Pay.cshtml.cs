using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ISP.Pages
{
    public class Pay : PageModel
    {
        public void OnPost()
        {
            // Properties are automatically set from the query parameters due to SupportsGet = true
        }
    }
}
