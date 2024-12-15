using System.ComponentModel.DataAnnotations;
using ISP.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ISP.Pages
{
    public class Reserve : PageModel
    {
        private readonly AppDbContext _context;
        public Reserve(AppDbContext context) => _context = context;

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        public Automobilis automobilis { get; set; }

        [BindProperty, DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        [BindProperty, DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }

        [BindProperty]
        public bool ShortTime { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            automobilis = await _context.Automobilis.FirstOrDefaultAsync(c => c.Id_Automobilis == Id);

            if (automobilis == null)
                return NotFound();

            return Page();
        }
    }
}
