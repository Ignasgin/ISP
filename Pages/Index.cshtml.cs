using ISP.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISP.Pages
{
    public class IndexModel : BaseModel
    {
        public IndexModel(AppDbContext context) : base(context)
        {
            
        }

        public IList<Atsiliepimas> Atsiliepimas { get; set; }
        public IList<Automobilis> Automobilis { get; set; }

        public IList<Draudimas> Draudimas { get; set; }

        public IList<Naudotojas> Naudotojas { get; set; }


        public async Task OnGetAsync()
        {
            Atsiliepimas = await _context.Atsiliepimas.ToListAsync();
            Draudimas = await _context.Draudimas.ToListAsync();
            Automobilis = await _context.Automobilis.ToListAsync();

            Naudotojas = await _context.Naudotojas.ToListAsync();


        }

    }
}
