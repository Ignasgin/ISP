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

        public async Task<IActionResult> OnPostAsync()
        {
            if (ShortTime)
            {
                Trumpalaike_Rezervacija trumpRezervacija = new Trumpalaike_Rezervacija()
                {
                    Pateikimo_Data = DateTime.Now,
                    Laikas = 1,
                    Fk_Automobilis_Id_Automobilis = Id,
                    Fk_Naudotojas_Id_Naudotojas = 0 // TODO set to user ID
                };

                _context.Trumpalaike_Rezervacija.Add(trumpRezervacija);
                await _context.SaveChangesAsync();

                return RedirectToPage("/Reservations");
            }

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

            return RedirectToPage("/Pay");
        }
    }
}
