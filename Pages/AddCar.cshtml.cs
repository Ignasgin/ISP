using ISP.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

public class AddCarModel : PageModel
{
    private readonly AppDbContext _context;

    [BindProperty]
    public Automobilis Car { get; set; }

    public AddCarModel(AppDbContext context)
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

        // Add the new car to the database
        _context.Automobilis.Add(Car);
        await _context.SaveChangesAsync();

        // Redirect to another page (for example, the cars list page)
        return RedirectToPage("/Cars");
    }
}

