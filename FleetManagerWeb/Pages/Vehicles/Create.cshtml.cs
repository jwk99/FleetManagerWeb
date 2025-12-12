using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FleetManagerWeb.Data;
using FleetManagerWeb.Models;

namespace FleetManagerWeb.Pages.Vehicles
{
    public class CreateModel : PageModel
    {
        private readonly FleetDbContext _db;

        public CreateModel(FleetDbContext db)
        {
            _db = db;
        }

        [BindProperty]
        public Vehicle Vehicle { get; set; }

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            Console.WriteLine($"Vehicle: {Vehicle.Brand}, {Vehicle.Model}, {Vehicle.Year}");

            _db.Vehicles.Add(Vehicle);
            await _db.SaveChangesAsync();

            return RedirectToPage("Index");
        }
    }
}
