using ISP.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using System.Linq;

namespace ISP.Pages
{
    
    public class LoginModel : PageModel
    {
        private readonly AppDbContext _context;

        [BindProperty]
        public Naudotojas User { get; set; }

        public LoginModel(AppDbContext context)
        {
            _context = context;
        }

        // Handle the POST request when the form is submitted
        public async Task<IActionResult> OnPostAsync()
        {

            User.Slaptazodis = Utils.ComputeMD5Hash(User.Slaptazodis);
            // Find the user from the database using the provided credentials
            var dbUser = _context.Naudotojas
                .FirstOrDefault(u => u.Vardas == User.Vardas &&
                                     u.Slaptazodis == User.Slaptazodis &&
                                     u.Pavarde == User.Pavarde &&
                                     u.Asmens_Kodas == User.Asmens_Kodas);

            // If the user was not found, show an error
            if (dbUser == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return Page();
            }
            dbUser.Paskutinis_Prisijungimas = DateTime.Now;
            await _context.SaveChangesAsync();

            // If the user was found, create the claims for the authenticated user
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, dbUser.Vardas + " " + dbUser.Pavarde),
                new Claim(ClaimTypes.NameIdentifier, dbUser.Id_Naudotojas.ToString())
            };

            // Create an identity based on the claims
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            // Sign the user in using cookies
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);



            // Redirect to the Index page (or any other page you want)
            return RedirectToPage("/Index");
        }
    }
}
