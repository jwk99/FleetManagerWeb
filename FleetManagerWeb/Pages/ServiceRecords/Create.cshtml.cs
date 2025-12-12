using FleetManagerWeb.Data;
using FleetManagerWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FleetManagerWeb.Pages.ServiceRecords
{
    public class CreateModel : PageModel
    {
        private readonly FleetDbContext _db;

        public CreateModel(FleetDbContext db)
        {
            _db = db;
        }

        [BindProperty]
        public ServiceRecord Record { get; set; }

        public List<SelectListItem> Vehicles { get; set; }
        public List<SelectListItem> Drivers { get; set; }

        public void OnGet()
        {
            Vehicles = new List<SelectListItem>();
            foreach (var v in _db.Vehicles.ToList())
            {
                Vehicles.Add(new SelectListItem
                {
                    Value = v.Id.ToString(),
                    Text = $"{v.Brand} {v.Model} ({v.Registration})"
                });
            }

            Drivers = new List<SelectListItem>();
            foreach (var d in _db.Drivers.ToList())
            {
                Drivers.Add(new SelectListItem
                {
                    Value = d.Id.ToString(),
                    Text = $"{d.FirstName} {d.LastName}"
                });
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Console.WriteLine($"VehicleId={Record.VehicleId}, DriverId={Record.DriverId}, Date={Record.Date}, Cost={Record.Cost}");
            if (!ModelState.IsValid)
            {
                Console.WriteLine("ModelState invalid:");
                foreach (var kv in ModelState)
                {
                    Console.WriteLine($"{kv.Key}: {string.Join(",", kv.Value.Errors.Select(e => e.ErrorMessage))}");
                }
            }
            if (Record.IsCompleted && Record.CompletedDate == null)
            {
                Record.CompletedDate = DateTime.Now;
            }

            _db.ServiceRecords.Add(Record);
            await _db.SaveChangesAsync();

            return RedirectToPage("Index");
        }
    }
}
