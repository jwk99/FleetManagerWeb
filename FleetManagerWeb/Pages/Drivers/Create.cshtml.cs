using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FleetManagerWeb.Data;
using FleetManagerWeb.Models;

namespace FleetManagerWeb.Pages.Drivers
{
    public class CreateModel : PageModel
    {
        private readonly FleetDbContext _db;
        public CreateModel(FleetDbContext db)
        {
            _db = db;
        }

        [BindProperty]
        public Driver Driver { get; set; }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();
            _db.Drivers.Add(Driver);
            await _db.SaveChangesAsync();

            return RedirectToPage("Index");
        }
    }
}
