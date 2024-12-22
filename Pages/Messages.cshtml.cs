using ISP.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ISP.Pages
{
    [Authorize]
    public class MessagesModel : BaseModel
    {

        public MessagesModel(AppDbContext context) : base(context)
        {

        }

        [BindProperty]
        public List<Pranesimas> Pranesimai { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {


            if (string.IsNullOrEmpty(UserID))
            {
                return BadRequest("Invalid ID provided.");
            }

            int.TryParse(UserID, out int id);

            // Fetch messages from the database where the user ID matches the given ID
            Pranesimai = await _context.Naudotojas
                .Where(m => m.Id_Naudotojas == id)
                .Include(d => d.pranesimas).SelectMany(x => x.pranesimas)
                .ToListAsync();



            if (Pranesimai == null || Pranesimai.Count == 0)
            {
                Pranesimai = new List<Pranesimas>();
            }

            return Page();
        }
    }
}
