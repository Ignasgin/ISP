using ISP.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace ISP.Pages.Services;

    public class EditServiceModel : PageModel
    {
        private readonly AppDbContext _context;

        [BindProperty]
        public Servisas Servisas { get; set; }

        public EditServiceModel(AppDbContext context)
        {
            _context = context;
        }

        // GET: /EditService/{id}
        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            // Pabandome gauti paslaugą pagal ID
            Servisas = await _context.Servisas.FirstOrDefaultAsync(s => s.Id_Servisas == id);

            if (Servisas == null)
            {
                return NotFound(); // Jei nerandame serviso, grąžiname klaidą
            }

            return Page();
        }

        // POST: /EditService/{id}
        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (!ModelState.IsValid)
            {
                return Page(); // Jei modelis neteisingas, grąžiname puslapį su klaidomis
            }

            var servisasToUpdate = await _context.Servisas.FindAsync(id);

            if (servisasToUpdate == null)
            {
                return NotFound();
            }

            // Atnaujiname serviso duomenis
            servisasToUpdate.Pavadinimas = Servisas.Pavadinimas;
            servisasToUpdate.Adresas = Servisas.Adresas;
            servisasToUpdate.Aprasymas = Servisas.Aprasymas;

            await _context.SaveChangesAsync(); // Išsaugome pakeitimus į duomenų bazę

            return RedirectToPage("/Services"); // Po sėkmingo atnaujinimo nukreipiame į paslaugų sąrašą
        }
    }
