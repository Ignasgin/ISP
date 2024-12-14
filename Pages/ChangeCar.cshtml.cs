using ISP.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class ChangeCarModel : PageModel
{
    private readonly AppDbContext _context;

    [BindProperty]
    public Automobilis Car { get; set; }
    public List<Draudimas> Insurances { get; set; }


    public ChangeCarModel(AppDbContext context)
    {
        _context = context;
    }

    // On GET, load the car details and insurances based on the carId query parameter
    public async Task<IActionResult> OnGetAsync(int carId)
    {
        Car = await _context.Automobilis
            .Include(c => c.Draudimai)
            .FirstOrDefaultAsync(c => c.Id_Automobilis == carId);

        Insurances = _context.Draudimas.Where(i => i.Fk_Automobilis_Id_Automobilis == carId).ToList();

        if (Car == null)
        {
            return NotFound();
        }

        return Page();
    }

    // On POST, save the updated car details and insurances to the database
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        _context.Attach(Car).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!CarExists(Car.Id_Automobilis))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return RedirectToPage("/Cars");
    }

    // Handler for editing an insurance
    public IActionResult OnPostUpdateInsurance(int insuranceId, int polisoNumeris, string draudimoTipas, string kompanija, DateTime startDate, DateTime endDate)
    {
        var insurance = _context.Draudimas.FirstOrDefault(i => i.Id_Draudimas == insuranceId);

        if (insurance != null)
        {
            insurance.Poliso_Numeris = polisoNumeris;
            insurance.Draudimo_Tipas = draudimoTipas;
            insurance.Draudimo_Kompanijos_Pavadinimas = kompanija;
            insurance.Pradzios_Data = startDate;
            insurance.Pabaigos_Data = endDate;

            _context.Draudimas.Update(insurance);
            _context.SaveChanges();
        }

        return RedirectToPage("/ChangeCar", new { carId = insurance.Fk_Automobilis_Id_Automobilis });
    }

    // Handler for deleting an insurance
    public IActionResult OnPostDeleteInsurance(int insuranceId)
    {
        var insurance = _context.Draudimas.FirstOrDefault(i => i.Id_Draudimas == insuranceId);

        if (insurance != null)
        {
            _context.Draudimas.Remove(insurance);
            _context.SaveChanges();
        }

        return RedirectToPage("/ChangeCar", new { carId = Car.Id_Automobilis });
    }
    // Handler for saving the updated car details
    public async Task<IActionResult> OnPostSaveCarAsync(int carId)
    {
        var carToUpdate = await _context.Automobilis.FirstOrDefaultAsync(c => c.Id_Automobilis == carId);
        if (carToUpdate != null)
        {
            // Update car details
            carToUpdate.Marke = Car.Marke;
            carToUpdate.Modelis = Car.Modelis;
            carToUpdate.Metai = Car.Metai;
            carToUpdate.Litrazas = Car.Litrazas;
            carToUpdate.Galia = Car.Galia;
            carToUpdate.Numeris = Car.Numeris;
            carToUpdate.Vin = Car.Vin;
            carToUpdate.Vietu_Sk = Car.Vietu_Sk;
            carToUpdate.Kuro_Tipas = Car.Kuro_Tipas;
            carToUpdate.Kebulo_Tipas = Car.Kebulo_Tipas;
            carToUpdate.Rida = Car.Rida;
            carToUpdate.Ivertinimu_Vidurkis = Car.Ivertinimu_Vidurkis;
            carToUpdate.Statusas = Car.Statusas;
            carToUpdate.Kaina = Car.Kaina;

            // Save the changes
            await _context.SaveChangesAsync();
        }

        return RedirectToPage("/Cars");
    }
    public async Task<IActionResult> OnPostAddInsuranceAsync(
    int carId,
    int polisoNumeris,
    string draudimoTipas,
    string kompanija,
    DateTime startDate,
    DateTime endDate)
    {
        // Find the car by the provided carId
        var car = await _context.Automobilis
            .FirstOrDefaultAsync(c => c.Id_Automobilis == carId);

        if (car == null)
        {
            // If car doesn't exist, return to the Cars page
            return RedirectToPage("/Cars");
        }

        // Create the new insurance object
        var newInsurance = new Draudimas
        {
            Fk_Automobilis_Id_Automobilis = carId,
            Poliso_Numeris = polisoNumeris,
            Draudimo_Tipas = draudimoTipas,
            Draudimo_Kompanijos_Pavadinimas = kompanija,
            Pradzios_Data = startDate,
            Pabaigos_Data = endDate
        };

        // Add the new insurance to the database
        _context.Draudimas.Add(newInsurance);

        // Save changes to the database
        await _context.SaveChangesAsync();

        // Redirect to the ChangeCar page to view the updated insurance list
        return RedirectToPage("/ChangeCar", new { carId = carId });
    }
    private bool CarExists(int id)
    {
        return _context.Automobilis.Any(e => e.Id_Automobilis == id);
    }
}
