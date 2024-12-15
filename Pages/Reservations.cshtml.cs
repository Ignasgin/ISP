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
        trumpalaikesRezervacijos = await _context.Trumpalaike_Rezervacija.ToListAsync();
        rezervacijos = await _context.Rezervacija.Select(r => new Rezervacija
        {
            Pradzia = r.Pradzia,
            Pabaiga = r.Pabaiga,
            Pateikimo_Data = r.Pateikimo_Data,
            Paemimo_Vieta = r.Paemimo_Vieta,
            Atidavimo_Vieta = r.Atidavimo_Vieta,
            Id_Rezervacija = r.Id_Rezervacija,
            Fk_Automobilis_Id_Automobilis = r.Fk_Automobilis_Id_Automobilis,
            Fk_Naudotojas_Id_Naudotojas = r.Fk_Naudotojas_Id_Naudotojas
        }).ToListAsync();

        return Page();
    }
}