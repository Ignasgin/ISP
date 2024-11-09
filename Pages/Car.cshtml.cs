using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ISP.Pages;

public class CarModel : PageModel
{
    [BindProperty(SupportsGet = true)]
    public string Make { get; set; }

    [BindProperty(SupportsGet = true)]
    public int Year { get; set; }
    
    [BindProperty(SupportsGet = true)]
    public double EngineSize { get; set; }

    [BindProperty(SupportsGet = true)]
    public string FuelType { get; set; }

    [BindProperty(SupportsGet = true)]
    public int Mileage { get; set; }

    [BindProperty(SupportsGet = true)]
    public string Grade { get; set; }

    [BindProperty(SupportsGet = true)]
    public string State { get; set; }

    [BindProperty(SupportsGet = true)]
    public string Price { get; set; }

    [BindProperty(SupportsGet = true)]
    public string Insurance { get; set; }
    
    public void OnGet()
    {
        
    }
}