using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FleetManagerWeb.Data;
using Microsoft.EntityFrameworkCore;
using FleetManagerWeb.Models;

namespace FleetManagerWeb.Pages.Vacations
{
    public class DeleteModel : PageModel
    {
        private readonly FleetDbContext _db;
        public DeleteModel(FleetDbContext db) => _db = db;

        [BindProperty]
        public Vacation Vacation { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Vacation = await _db.Vacations
                .Include(v => v.Driver)
                .Include(v => v.SubstituteDriver)
                .FirstOrDefaultAsync(v => v.Id == id);

            if (Vacation == null) 
                return NotFound();

            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            var vacation = await _db.Vacations.FindAsync(Vacation.Id);
            if (vacation == null) return NotFound();

            _db.Vacations.Remove(vacation);
            await _db.SaveChangesAsync();

            return RedirectToPage("Index");
        }
    }
}
