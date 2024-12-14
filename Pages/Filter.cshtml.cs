using ISP.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

public class FilterModel : PageModel
{
    private readonly AppDbContext _context;

    public FilterModel(AppDbContext context)
    {
        _context = context;
    }

    [BindProperty]
    public string Marke { get; set; }
    [BindProperty]
    public string Modelis { get; set; }
    [BindProperty]
    public string Metai { get; set; }
    [BindProperty]
    public string Litrazas { get; set; }
    [BindProperty]
    public string Galia { get; set; }
    [BindProperty]
    public string Numeris { get; set; }
    [BindProperty]
    public string Vin { get; set; }
    [BindProperty]
    public string VietuSk { get; set; }
    [BindProperty]
    public string KebuloTipas { get; set; }
    [BindProperty]
    public string KuroTipas { get; set; }
    [BindProperty]
    public string Rida { get; set; }
    [BindProperty]
    public string Ivertinimas { get; set; }
    [BindProperty]
    public string Statusas { get; set; }
    [BindProperty]
    public string Kaina { get; set; }
    [BindProperty]
    public string Draudimas { get; set; }

    // For storing the filtered cars
    public List<Automobilis> FilteredCars { get; set; }

    // Dropdown values
    public List<string> Markenes { get; set; }
    public List<string> Modeliai { get; set; }
    public List<int> Metais { get; set; }
    public List<double?> Litrazai { get; set; }
    public List<int> Galios { get; set; }
    public List<string> Numeriai { get; set; }
    public List<string> Vinai { get; set; }
    public List<int> VietuSkai { get; set; }
    public List<string> KebuloTipai { get; set; }
    public List<string> KuroTipai { get; set; }
    public List<int> Ridos { get; set; }
    public List<double?> IvertinimaiList { get; set; }
    public List<string> Statusai { get; set; }
    public List<decimal> Kainos { get; set; }
    public List<string> DraudimaiList { get; set; }

    public void OnGet()
    {
        // Fetch distinct values from the database for dropdown lists
        Markenes = _context.Automobilis.Select(c => c.Marke).Distinct().ToList();
        Modeliai = _context.Automobilis.Select(c => c.Modelis).Distinct().ToList();
        Galios = _context.Automobilis.Select(c => c.Galia).Distinct().ToList();
        Numeriai = _context.Automobilis.Select(c => c.Numeris).Distinct().ToList();
        Vinai = _context.Automobilis.Select(c => c.Vin).Distinct().ToList();
        VietuSkai = _context.Automobilis.Select(c => c.Vietu_Sk).Distinct().ToList();
        KebuloTipai = _context.Automobilis.Select(c => c.Kebulo_Tipas).Distinct().ToList();
        Metais = _context.Automobilis.Select(c => c.Metai).Distinct().ToList();
        Litrazai = _context.Automobilis.Select(c => c.Litrazas).Distinct().ToList();
        KuroTipai = _context.Automobilis.Select(c => c.Kuro_Tipas).Distinct().ToList();
        Ridos = _context.Automobilis.Select(c => c.Rida).Distinct().ToList();
        IvertinimaiList = _context.Automobilis.Select(c => c.Ivertinimu_Vidurkis).Distinct().ToList();
        Statusai = _context.Automobilis.Select(c => c.Statusas).Distinct().ToList();
        Kainos = _context.Automobilis.Select(c => c.Kaina).Distinct().ToList();
        DraudimaiList = _context.Draudimas.Select(d => d.Draudimo_Tipas).Distinct().ToList();
    }



    public async Task<IActionResult> OnPostAsync()
    {
        var url = "/Cars?";

        // Build the query string based on selected filter values
        if (!string.IsNullOrEmpty(Marke) && Marke != "base") url += $"Marke={Marke}&";
        if (!string.IsNullOrEmpty(Modelis) && Modelis != "base") url += $"Modelis={Modelis}&";
        if (!string.IsNullOrEmpty(Metai) && Metai != "base") url += $"Metai={Metai}&";
        if (!string.IsNullOrEmpty(Galia) && Galia != "base") url += $"Galia={Galia}&";
        if (!string.IsNullOrEmpty(Numeris) && Numeris != "base") url += $"Numeris={Numeris}&";
        if (!string.IsNullOrEmpty(Vin) && Vin != "base") url += $"Vin={Vin}&";
        if (!string.IsNullOrEmpty(VietuSk) && VietuSk != "base") url += $"VietuSk={VietuSk}&";
        if (!string.IsNullOrEmpty(KebuloTipas) && KebuloTipas != "base") url += $"KebuloTipas={KebuloTipas}&";
        if (!string.IsNullOrEmpty(Litrazas) && Litrazas != "base") url += $"Litrazas={Litrazas}&";
        if (!string.IsNullOrEmpty(KuroTipas) && KuroTipas != "base") url += $"KuroTipas={KuroTipas}&";
        if (!string.IsNullOrEmpty(Rida) && Rida != "base") url += $"Rida={Rida}&";
        if (!string.IsNullOrEmpty(Ivertinimas) && Ivertinimas != "base" && Ivertinimas != " ") url += $"Ivertinimas={Ivertinimas}&";
        if (!string.IsNullOrEmpty(Statusas) && Statusas != "base") url += $"Statusas={Statusas}&";
        if (!string.IsNullOrEmpty(Kaina) && Kaina != "base") url += $"Kaina={Kaina}&";
        if (!string.IsNullOrEmpty(Draudimas) && Draudimas != "base") url += $"Draudimas={Draudimas}&";
        url = url.TrimEnd('&');
        // Redirect to the filtered cars page with the query string
        return Redirect(url);
    }

}
