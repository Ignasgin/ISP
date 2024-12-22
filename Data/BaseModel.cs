using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;


namespace ISP.Data
{
    public class BaseModel: PageModel
    {
        internal readonly AppDbContext _context;
        public bool IsAuthenticated { get; set; }
        public string Username { get; set; }
        public string? UserID { get; set; }

        public BaseModel(AppDbContext context)
        {
            _context = context;
        }
        public override void OnPageHandlerExecuting(PageHandlerExecutingContext context)
        {
            base.OnPageHandlerExecuting(context);
            IsAuthenticated = User.Identity?.IsAuthenticated ?? false;
            Username = User.Identity?.Name;
            UserID = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
        }
    }
}
