using System.Diagnostics;
using ISP.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ISP.Pages;

public class CarModel : PageModel
{
    private readonly AppDbContext _context;

    public CarModel(AppDbContext context) => _context = context;

    [BindProperty(SupportsGet = true)]
    public int Id { get; set; }

    public Automobilis automobilis { get; set; }

    public async Task<IActionResult> OnGetAsync() {
        automobilis = await _context.Automobilis.FirstOrDefaultAsync(c => c.Id_Automobilis == Id);

        if (automobilis == null)
            return NotFound();

        return Page();
    }
}