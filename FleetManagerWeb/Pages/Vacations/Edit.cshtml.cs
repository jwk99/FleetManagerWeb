using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FleetManagerWeb.Data;
using Microsoft.EntityFrameworkCore;
using FleetManagerWeb.Models;

namespace FleetManagerWeb.Pages.Vacations
{
    public class EditModel : PageModel
    {
        private readonly FleetDbContext _db;
        public EditModel(FleetDbContext db) => _db = db;
        [BindProperty]
        public Vacation Vacation { get; set; }
        public IList<Driver> Drivers { get; set; }
        public async Task<IActionResult> OnGetAsync(int id)
        {
            Vacation = await _db.Vacations.FindAsync(id);
            if (Vacation == null) return NotFound();

            Drivers = await _db.Drivers.ToListAsync();
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            Drivers = await _db.Drivers.ToListAsync();

            if (Vacation.From > Vacation.To)
            {
                ModelState.AddModelError("", "Data koñca nie mo¿e byæ wczeœniejsza ni¿ data pocz¹tku.");
                return Page();
            }
            bool overlapMain = await _db.Vacations
                .Where(v => v.DriverId == Vacation.DriverId && v.Id != Vacation.Id)
                .AnyAsync(v => v.From <= Vacation.To && v.To >= Vacation.From);

            if (overlapMain)
            {
                ModelState.AddModelError("Vacation.DriverId", "Ten kierowca ma ju¿ urlop w tym okresie.");
                return Page();
            }
            if (Vacation.SubstituteDriverId != null)
            {
                bool overlapSub = await _db.Vacations
                    .Where(v => v.DriverId == Vacation.SubstituteDriverId && v.Id != Vacation.Id)
                    .AnyAsync(v => v.From <= Vacation.To && v.To >= Vacation.From);

                if (overlapSub)
                {
                    ModelState.AddModelError("Vacation.SubstituteDriverId",
                        "Kierowca zastêpuj¹cy jest ju¿ na urlopie w tym czasie.");
                    return Page();
                }
            }
            _db.Vacations.Update(Vacation);
            await _db.SaveChangesAsync();
            return RedirectToPage("Index");
        }
    }
}
