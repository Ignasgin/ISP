using ISP.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

public class Reservations : PageModel
{
    private readonly AppDbContext _context;

    public Reservations(AppDbContext context) => _context = context;

    public List<Trumpalaike_Rezervacija> trumpalaikesRezervacijos { get; set; }
    public List<Rezervacija> rezervacijos { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        trumpalaikesRezervacijos = await _context.Trumpalaike_Rezervacija.Include(r => r.Automobilis).ToListAsync();
        rezervacijos = await _context.Rezervacija.Include(r => r.Automobilis).ToListAsync();

        return Page();
    }
}