using ISP.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text;

namespace ISP.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly AppDbContext _context;

        [BindProperty]
        public Naudotojas User { get; set; }

        public RegisterModel(AppDbContext context)
        {
            _context = context;
        }

        // Handle the POST request when the form is submitted
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Add the new car to the database
            User.Slaptazodis = Utils.ComputeMD5Hash(User.Slaptazodis);
            // User.Registracijos_Data = DateOnly.FromDateTime(DateTime.Now);
            User.Registracijos_Data = DateTime.Now;
            _context.Naudotojas.Add(User);
            await _context.SaveChangesAsync();

            // Redirect to another page (for example, the cars list page)
            return RedirectToPage("/Index");
        }


    }
}



