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
    public string Marke { get; set; }
    public string Modelis { get; set; }
    public string Metai { get; set; }
    public string Galia { get; set; }
    public string Numeris { get; set; }
    public string Vin { get; set; }
    public string VietuSk { get; set; }
    public double? Litrazas { get; set; }
    public string KebuloTipas { get; set; }
    public double? Ivertinimu_Vidurkis { get; set; }
    public string KuroTipas { get; set; }
    public string Rida { get; set; }
    public string Statusas { get; set; }
    public string Kaina { get; set; }
    public List<Automobilis> Cars { get; set; }

    public async Task<IActionResult> OnGetAsync(
        string Marke, string Modelis, string Metai, string Galia, string Numeris, string Vin,
        string VietuSk, double? Litrazas, string KebuloTipas, double? Ivertinimu_Vidurkis, string KuroTipas,
        string Rida, string Statusas, string Kaina)
    {
        // Initial query setup
        var query = _context.Automobilis.Include(c => c.Draudimai).AsQueryable();

        // Apply filters based on the query parameters
        if (!string.IsNullOrEmpty(Marke) && Marke != "base")
            query = query.Where(c => c.Marke == Marke);

        if (!string.IsNullOrEmpty(Modelis) && Modelis != "base")
            query = query.Where(c => c.Modelis == Modelis);

        if (!string.IsNullOrEmpty(Galia) && Galia != "base")
            query = query.Where(c => c.Galia.ToString() == Galia);

        if (!string.IsNullOrEmpty(Numeris) && Numeris != "base")
            query = query.Where(c => c.Numeris == Numeris);

        if (!string.IsNullOrEmpty(Vin) && Vin != "base")
            query = query.Where(c => c.Vin == Vin);

        if (!string.IsNullOrEmpty(VietuSk) && VietuSk != "base")
            query = query.Where(c => c.Vietu_Sk.ToString() == VietuSk);

        if (!string.IsNullOrEmpty(KebuloTipas) && KebuloTipas != "base")
            query = query.Where(c => c.Kebulo_Tipas == KebuloTipas);

        if (!string.IsNullOrEmpty(Metai) && Metai != "base")
            query = query.Where(c => c.Metai.ToString() == Metai);

        if (Litrazas.HasValue)
            query = query.Where(c => c.Litrazas == Litrazas);

        if (!string.IsNullOrEmpty(KuroTipas) && KuroTipas != "base")
            query = query.Where(c => c.Kuro_Tipas == KuroTipas);

        if (!string.IsNullOrEmpty(Rida) && Rida != "base")
            query = query.Where(c => c.Rida.ToString() == Rida);

        if (Ivertinimu_Vidurkis.HasValue)
            query = query.Where(c => c.Ivertinimu_Vidurkis == Ivertinimu_Vidurkis);

        if (!string.IsNullOrEmpty(Statusas) && Statusas != "base")
            query = query.Where(c => c.Statusas == Statusas);

        if (!string.IsNullOrEmpty(Kaina) && Kaina != "base")
            query = query.Where(c => c.Kaina.ToString() == Kaina);

        // Fetch the filtered cars from the database
        Cars = await query.ToListAsync();

        return Page();
    }
}
