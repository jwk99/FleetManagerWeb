using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FleetManagerWeb.Data;
using Microsoft.EntityFrameworkCore;
using FleetManagerWeb.Models;


namespace FleetManagerWeb.Pages.Vacations
{
    public class DetailsModel : PageModel
    {
        private readonly FleetDbContext _db;
        public DetailsModel(FleetDbContext db) => _db = db;

        public Vacation Vacation { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Vacation = await _db.Vacations
                .Include(v => v.Driver)
                .Include(v => v.SubstituteDriver)
                .FirstOrDefaultAsync(v => v.Id == id);

            if (Vacation == null) return NotFound();

            return Page();
        }
    }
}
