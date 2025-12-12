using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FleetManagerWeb.Data;
using Microsoft.EntityFrameworkCore;
using FleetManagerWeb.Models;

namespace FleetManagerWeb.Pages.Vacations
{
    public class CreateModel : PageModel
    {
        private readonly FleetDbContext _db;
        public CreateModel(FleetDbContext db) => _db = db;
        [BindProperty]
        public Vacation Vacation { get; set; }
        public IList<Driver> Drivers { get; set; }
        public async Task OnGetAsync()
        {
            Drivers = await _db.Drivers.ToListAsync();
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
                .Where(v => v.DriverId == Vacation.DriverId)
                .AnyAsync(v => v.From <= Vacation.To && v.To >= Vacation.From);
            if (overlapMain)
            {
                ModelState.AddModelError("Vacation.DriverId", "Ten kierowca ma ju¿ urlop w tym okresie.");
                return Page();
            }
            if (Vacation.SubstituteDriverId != null)
            {
                bool overlapSub = await _db.Vacations
                    .Where(v => v.DriverId == Vacation.SubstituteDriverId)
                    .AnyAsync(v => v.From <= Vacation.To && v.To >= Vacation.From);

                if (overlapSub)
                {
                    ModelState.AddModelError("Vacation.SubstituteDriverId",
                        "Wybrany kierowca zastêpuj¹cy jest w tym czasie na urlopie.");
                    return Page();
                }
            }
            _db.Vacations.Add(Vacation);
            await _db.SaveChangesAsync();
            return RedirectToPage("Index");
        }
    }
}
