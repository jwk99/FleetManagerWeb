using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FleetManagerWeb.Data;
using FleetManagerWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace FleetManagerWeb.Pages.Vehicles
{
    public class EditModel : PageModel
    {
        private readonly FleetDbContext _db;

        public EditModel(FleetDbContext db)
        {
            _db = db;
        }

        [BindProperty]
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
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();
            _db.Vehicles.Update(Vehicle);
            await _db.SaveChangesAsync();
            return RedirectToPage("Index");
        }
    }
}