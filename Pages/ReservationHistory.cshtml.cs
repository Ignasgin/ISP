using ISP.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ISP.Pages
{
    [Authorize]
    public class ReservationHistoryModel : BaseModel
    {
        public ReservationHistoryModel(AppDbContext context) : base(context)
        {
        }


        [BindProperty]
        public List<Rezervacija> Rezervacija { get; set; }


        public async Task<IActionResult> OnGetAsync()
        {


            if (string.IsNullOrEmpty(UserID))
            {
                return BadRequest("Invalid ID provided.");
            }

            int.TryParse(UserID, out int id);

            // Fetch messages from the database where the user ID matches the given ID
            Rezervacija = await _context.Rezervacija
                .Where(m => m.Fk_Naudotojas_Id_Naudotojas == id)
                .Include(d => d.Automobilis)
                .ToListAsync();



            if (Rezervacija == null || Rezervacija.Count == 0)
            {
                Rezervacija = new List<Rezervacija>();
            }

            return Page();
        }
    }
}
