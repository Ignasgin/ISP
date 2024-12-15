using System.Diagnostics;
using ISP.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ISP.Pages;

public class CarModel : PageModel
{
    private readonly AppDbContext _context;

    public CarModel(AppDbContext context) => _context = context;

    [BindProperty(SupportsGet = true)] public int Id { get; set; }

    public Automobilis? Automobilis { get; set; }

    [BindProperty] public Atsiliepimas Atsiliepimas { get; set; } = new();

    public List<Atsiliepimas>? Atsiliepimai { get; set; } = new();

    public Naudotojas CurrentUser { get; set; } = new(); // GetCurrentUser()
    

    public async Task<IActionResult> OnGetAsync(int id)
    {
        CurrentUser.Id_Naudotojas = 1;
        Automobilis = await _context.Automobilis
            .Include(a => a.Atsiliepimai)!
            .ThenInclude(at => at.Naudotojas)
            .FirstOrDefaultAsync(a => a.Id_Automobilis == id);

        if (Automobilis == null)
        {
            return NotFound();
        }

        var watchedAuto = new Perziuretas_Automobilis
        {
            Data = DateTime.UtcNow,
            Fk_Automobilis_Id_Automobilis = Id,
            Fk_Naudotojas_Id_Naudotojas = CurrentUser.Id_Naudotojas,
        };

        var isExist = await _context.Perziuretas_Automobilis.AsQueryable()
            .AnyAsync(x =>
                x.Fk_Automobilis_Id_Automobilis == Id && x.Fk_Naudotojas_Id_Naudotojas == CurrentUser.Id_Naudotojas);

        if (!isExist)
        {
            await _context.Perziuretas_Automobilis.AddAsync(watchedAuto);
            await _context.SaveChangesAsync();
        }

        Automobilis.Kaina = Math.Round(Automobilis.Kaina, 2);
        Atsiliepimai = Automobilis.Atsiliepimai?.ToList() ?? new List<Atsiliepimas>();


        return Page();
    }


    public async Task<IActionResult> OnPostAddReview(int id)
    {
        Automobilis = await _context.Automobilis
            .Include(a => a.Atsiliepimai)!
            .ThenInclude(at => at.Naudotojas)
            .FirstOrDefaultAsync(a => a.Id_Automobilis == id);

        if (Automobilis == null)
        {
            return NotFound();
        }

        ModelState.Clear();
        if (string.IsNullOrWhiteSpace(Atsiliepimas.Komentaras))
        {
            ModelState.AddModelError("Atsiliepimas.Komentaras", "Komentaras yra privalomas.");
        }

        if (!ModelState.IsValid)
        {
            Atsiliepimai = Automobilis.Atsiliepimai?.ToList() ?? new List<Atsiliepimas>();
            return Page();
        }

        Atsiliepimas.Data = DateTime.UtcNow;
        Atsiliepimas.Titulas = "Naudotojas";
        Atsiliepimas.Fk_Automobilis_Id_Automobilis = Automobilis.Id_Automobilis;

        Atsiliepimas.Fk_Naudotojas_Id_Naudotojas = 1;

        await _context.Atsiliepimas.AddAsync(Atsiliepimas);
        await _context.SaveChangesAsync();
        Debug.WriteLine($"Komentaras: {Atsiliepimas.Komentaras}");


        UpdateAverageRating(Atsiliepimas.Ivertinimas);

        return RedirectToPage(new { id });
    }


    public void UpdateAverageRating(int newValue)
    {
        if (Automobilis?.Atsiliepimai == null)
        {
            throw new InvalidOperationException("Automobilis or its reviews cannot be null.");
        }

        var currentRatings = Automobilis.Atsiliepimai.Select(r => r.Ivertinimas).ToList();

        // Recalculate the average
        Automobilis.Ivertinimu_Vidurkis = (currentRatings.Count > 0
            ? Math.Round((double)currentRatings.Sum() / currentRatings.Count, 2)
            : 0);

        _context.Entry(Automobilis).State = EntityState.Modified;
        _context.SaveChanges();
    }

    public async Task<IActionResult> OnPostEditReview(int id)
    {
        Automobilis = await _context.Automobilis
            .Include(a => a.Atsiliepimai)!
            .ThenInclude(at => at.Naudotojas)
            .FirstOrDefaultAsync(a => a.Id_Automobilis == id);

        if (Automobilis == null)
        {
            return NotFound();
        }

        var existingReview = await _context.Atsiliepimas.FindAsync(Atsiliepimas.Id_Atsiliepimas);
        if (existingReview == null)
        {
            return Unauthorized(); // Ensure only the owner can edit their review.
        }

        ModelState.Clear();
        if (string.IsNullOrWhiteSpace(Atsiliepimas.Komentaras))
        {
            ModelState.AddModelError("Atsiliepimas.Komentaras", "Komentaras yra privalomas.");
        }

        if (!ModelState.IsValid)
        {
            Atsiliepimai = Automobilis.Atsiliepimai?.ToList() ?? new List<Atsiliepimas>();
            return Page();
        }

        // Update the review
        existingReview.Komentaras = Atsiliepimas.Komentaras;
        existingReview.Ivertinimas = Atsiliepimas.Ivertinimas;
        existingReview.Data = DateTime.UtcNow;
        existingReview.Rekomenduotu_Kitiems = Atsiliepimas.Rekomenduotu_Kitiems;

        _context.Entry(existingReview).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        // Recalculate the average rating
        UpdateAverageRating(Atsiliepimas.Ivertinimas);

        return RedirectToPage(new { id });
    }


    public async Task<List<Automobilis>> GetTop3CarsByRatingAsync()
    {
        return await _context.Automobilis.AsQueryable().Where(a => a.Id_Automobilis != Id)
            .OrderByDescending(a => a.Ivertinimu_Vidurkis).Take(3)
            .ToListAsync();
    }

    public async Task<List<Automobilis>> GetTop3SimilarCarsAsync()
    {
        var similarityCalculator = new CarSimilarity(_context);
        var similarCars = await similarityCalculator.FindTop3SimilarCarsAsync(Id);

        if (similarCars.Count == 0)
            return await GetTop3CarsByRatingAsync();
        
        foreach (var similarCar in similarCars)
        {
            Console.WriteLine($"Car ID: {similarCar.SimilarCarId}, Similarity: {similarCar.Similarity:F2}");
        }

        // Fetch all cars from the database
        var allCars = await _context.Automobilis.ToListAsync();

        // Filter the top 3 similar cars on the client side
        var top3Cars = allCars
            .Where(car => similarCars.Any(sc => sc.SimilarCarId == car.Id_Automobilis))
            .ToList();

        return top3Cars;
    }

    public async Task<List<Automobilis>> GetTop3HistoryCarsAsync()
    {
        // Instantiate the similarity calculator and fetch similar cars
        var similarityCalculator = new CarSimilarityHistory(_context);
        var similarCars = await similarityCalculator.FindTop3SimilarCarsAsync(Id, CurrentUser.Id_Naudotojas);

        if (similarCars.Count == 0)
            return await GetTop3CarsByRatingAsync();
        
        foreach (var similarCar in similarCars)
        {
            Console.WriteLine($"Car ID: {similarCar.SimilarCarId}, Similarity: {similarCar.WeightedSimilarity:F2}");
        }

        // Fetch only the top 3 similar cars using their IDs
        var top3CarIds = similarCars.Select(sc => sc.SimilarCarId).ToList();

        var top3Cars = await _context.Automobilis
            .Where(car => top3CarIds.Contains(car.Id_Automobilis))
            .ToListAsync();

        return top3Cars;
    }

    public async Task<bool> IsUserBoughtCarAndIsFirstComment()
    {
        var reservations = await _context.Rezervacija.AsQueryable().Where(x =>
                x.Fk_Automobilis_Id_Automobilis == Id && x.Fk_Naudotojas_Id_Naudotojas == CurrentUser.Id_Naudotojas)
            .ToListAsync();

        if (reservations.Count == 0)
            return false;

        var comment = await _context.Atsiliepimas.AsQueryable()
            .Where(x => x.Fk_Naudotojas_Id_Naudotojas == CurrentUser.Id_Naudotojas).FirstOrDefaultAsync();
        
        return comment == null;
    }
}