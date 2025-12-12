using FleetManagerWeb.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FleetManagerWeb.Models;

namespace FleetManagerWeb.Pages.Drivers
{
    public class EditModel : PageModel
    {
        private readonly FleetDbContext _db;
        public EditModel(FleetDbContext db)
        {
            _db = db;
        }
        [BindProperty]
        public Driver Driver { get; set; }
        public async Task<IActionResult> OnGetAsync(int id)
        {
            Driver = await _db.Drivers.FindAsync(id);

            if (Driver == null)
                return RedirectToPage("Index");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if(!ModelState.IsValid)
                return Page();
            _db.Drivers.Update(Driver);
            await _db.SaveChangesAsync();

            return RedirectToPage("Index");
        }
    }
}
