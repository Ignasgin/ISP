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
        public DateTime StartDate { get; set; } = DateTime.Now;

        [BindProperty, DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; } = DateTime.Now.AddDays(1);

        [BindProperty]
        public bool ShortTime { get; set; }

        private async Task<bool> CheckIsReserved (DateTime startDate, DateTime endDate)
        {
            bool reservuotas = await _context.Rezervacija.AnyAsync(r => r.Pradzia <= endDate && r.Pabaiga >= startDate && r.Fk_Automobilis_Id_Automobilis == Id);

            if (!reservuotas)
            {
                return await _context.Trumpalaike_Rezervacija.AnyAsync(r => r.Pateikimo_Data <= endDate && r.Pateikimo_Data.AddHours(r.Laikas) >= startDate && r.Fk_Automobilis_Id_Automobilis == Id);
            }

            return true;
        }

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
                if (await CheckIsReserved(DateTime.Now, DateTime.Now.AddHours(1)))
                {
                    ModelState.AddModelError("StartDate", "Automobilis šiuo laikotarpiu užimtas");
                    await OnGetAsync();
                    return Page();
                }

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

            if (await CheckIsReserved(StartDate, EndDate))
                {
                ModelState.AddModelError("StartDate", "Automobilis šiuo laikotarpiu užimtas");
                await OnGetAsync();
                return Page();
            }

            return RedirectToPage("/Pay", new {
                Id = Id,
                StartDate = StartDate.ToString("yyyy-MM-ddTHH:mm"),
                EndDate = EndDate.ToString("yyyy-MM-ddTHH:mm")
            });
        }
    }
}
