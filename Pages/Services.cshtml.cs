using ISP.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISP.Pages
{
    public class ServicesModel : PageModel
    {
        private readonly AppDbContext _context;

        public ServicesModel(AppDbContext context)
        {
            _context = context;
        }
            public IList<Servisas> Servisai { get; set; }

        public async Task OnGetAsync()
{
    // Užkrauname servisus su jų paslaugomis per tarpinę lentelę
    Servisai = await _context.Servisas
        .Include(s => s.ServicePaslaugos)
        .ThenInclude(sp => sp.Paslauga) // Užkrauname susijusias paslaugas
        .ToListAsync();
}
    }
}