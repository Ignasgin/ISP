using ISP.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ISP.Pages.Services;

    public class DeleteServiceModel : PageModel
    {
        private readonly AppDbContext _context;

        public DeleteServiceModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Servisas Servisas { get; set; }

        // This method loads the service to be deleted
        public async Task<IActionResult> OnGetAsync(int id)
        {
            // Fetch the service using the ID from the URL
            Servisas = await _context.Servisas
                .FirstOrDefaultAsync(s => s.Id_Servisas == id);

            // If service is not found, return NotFound page
            if (Servisas == null)
            {
                return NotFound();
            }

            return Page();
        }

        // This method deletes the service from the database
        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            // Retrieve the service from the database
            var servisasToDelete = await _context.Servisas.FindAsync(id);

            // If service does not exist in the database, return NotFound
            if (servisasToDelete == null)
            {
                return NotFound();
            }

            // Remove the service from the database
            _context.Servisas.Remove(servisasToDelete);
            await _context.SaveChangesAsync();

            // Redirect to the list of services after deletion
            return RedirectToPage("/Services");
        }
    }