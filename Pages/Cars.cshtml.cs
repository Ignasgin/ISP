using ISP.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

public class CarsModel : PageModel
{
    private readonly AppDbContext _context;

    public CarsModel(AppDbContext context)
    {
        _context = context;
    }

    public List<Automobilis> Cars { get; set; }

    public async Task OnGetAsync()
    {
        Cars = await _context.Automobilis.ToListAsync();
    }
}
