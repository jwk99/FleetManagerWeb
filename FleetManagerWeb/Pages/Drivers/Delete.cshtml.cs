using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FleetManagerWeb.Models;
using FleetManagerWeb.Data;

namespace FleetManagerWeb.Pages.Drivers
{
    public class DeleteModel : PageModel
    {
        private readonly FleetDbContext _db;
        public DeleteModel(FleetDbContext db)
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
            var d = await _db.Drivers.FindAsync(Driver.Id);

            if(d!=null)
            {
                _db.Drivers.Remove(d);
                await _db.SaveChangesAsync();
            }
            return RedirectToPage("Index");
        }
    }
}
