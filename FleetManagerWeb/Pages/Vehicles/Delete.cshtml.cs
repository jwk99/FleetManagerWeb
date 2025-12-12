using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FleetManagerWeb.Data;
using FleetManagerWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace FleetManagerWeb.Pages.Vehicles
{
    public class DeleteModel : PageModel
    {
        private readonly FleetDbContext _db;

        public DeleteModel(FleetDbContext db)
        {
            _db = db;
        }

        public Vehicle Vehicle { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Vehicle = await _db.Vehicles.FirstOrDefaultAsync(v => v.Id == id);

            if (Vehicle == null)
            {
                return RedirectToPage("Index");
            }
            return Page();
        }
        public async Task<IActionResult> OnPostAsync(int id)
        {
            var vehicle = await _db.Vehicles.FindAsync(id);

            if (vehicle !=null)
            {
                _db.Vehicles.Remove(vehicle);
                await _db.SaveChangesAsync();
            }
            return RedirectToPage("Index");
        }
    }
}
