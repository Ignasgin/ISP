using ISP.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class AddServiceModel : PageModel
{
    private readonly AppDbContext _context;

    // Reikalingi parametrai servisui ir paslaugoms
    [BindProperty]
    public Servisas Service { get; set; }
    [BindProperty]
    public List<string> Services { get; set; }
    [BindProperty]
    public List<double> Prices { get; set; }
    [BindProperty]
    public List<int> Times { get; set; }

    public AddServiceModel(AppDbContext context)
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

        // Pridėti servisą į duomenų bazę
        _context.Servisas.Add(Service);
        await _context.SaveChangesAsync();

        // Pridėti paslaugas į duomenų bazę
        for (int i = 0; i < Services.Count; i++)
        {
            var paslauga = new Paslauga
            {
                Pavadinimas = Services[i],
                kaina = Prices[i],
                laikas = Times[i]
            };

            _context.Paslauga.Add(paslauga);
            await _context.SaveChangesAsync();

            // Sukurti nuorodą tarp serviso ir paslaugos
            var servisuPaslaugos = new Servisu_paslaugos
            {
                fk_Servisas_id_Servisas = Service.Id_Servisas,
                fk_Paslauga_id_Paslauga = paslauga.id_Paslauga
            };
            _context.Servisu_paslaugos.Add(servisuPaslaugos);
        }

        // Išsaugoti visus pakeitimus
        await _context.SaveChangesAsync();

        // Nukreipti į serviso puslapį po sėkmingo įrašymo
        return RedirectToPage("/Services");
    }
}