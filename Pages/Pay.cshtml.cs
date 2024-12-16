using ISP.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ISP.Pages
{
    public class Pay : PageModel
    {
        private readonly AppDbContext _context;

        public Pay(AppDbContext context) => _context = context;

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime StartDate { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime EndDate { get; set; }

        public Automobilis automobilis { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            automobilis = await _context.Automobilis.FirstOrDefaultAsync(c => c.Id_Automobilis == Id);

            if (automobilis == null)
                return NotFound();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Rezervacija rezervacija = new Rezervacija()
            {
                Pradzia = StartDate,
                Pabaiga = EndDate,
                Pateikimo_Data = DateTime.Now,
                Paemimo_Vieta = "",
                Atidavimo_Vieta = "",
                Fk_Automobilis_Id_Automobilis = Id,
                Fk_Naudotojas_Id_Naudotojas = 0 // TODO set to user ID
            };

            _context.Rezervacija.Add(rezervacija);
            await _context.SaveChangesAsync();

            var tempAuto = await _context.Automobilis.FirstOrDefaultAsync(a => a.Id_Automobilis == Id);

            if (tempAuto != null)
            {
                // Update the status column
                tempAuto.Statusas = "Rezervuota"; // Replace "NewStatus" with the desired value

                // Save changes to the database
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("/Reservations");
        }
    }
}
