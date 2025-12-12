using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FleetManagerWeb.Data;
using Microsoft.EntityFrameworkCore;

namespace FleetManagerWeb.Pages.Drivers
{
    public class ProfileModel : PageModel
    {
        private readonly FleetDbContext _db;
        public ProfileModel(FleetDbContext db)
        {
            _db = db;
        }
        public class DriverProfileVm
        {
            public int DriverId { get; set; }
            public string Name { get; set; }
            public string Phone { get; set; }
            public string LicenseNumber { get; set; }

            public int TotalServices { get; set; }
            public decimal TotalCost { get; set; }
            public decimal AverageCost { get; set; }

            public List<ServiceItem> Services { get; set; }
        }

        public class ServiceItem
        {
            public DateTime Date { get; set; }
            public string Vehicle {  get; set; }
            public string Description { get; set; }
            public decimal Cost { get; set; }
        }

        public DriverProfileVm Driver {  get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var driver = await _db.Drivers.FirstOrDefaultAsync(d => d.Id == id);
            if (driver == null)
                return NotFound();
            var services = await _db.ServiceRecords
                .Where(s => s.DriverId == id)
                .Include(s => s.Vehicle)
                .OrderByDescending(s => s.Date)
                .ToListAsync();
            Driver = new DriverProfileVm
            {
                DriverId = driver.Id,
                Name = $"{driver.FirstName} {driver.LastName}",
                Phone = driver.Phone,
                LicenseNumber = driver.LicenseNumber,

                TotalCost = services.Sum(s => s.Cost),
                TotalServices = services.Count,
                AverageCost = services.Any() ? services.Average(s => s.Cost) : 0,

                Services = services.Select(s => new ServiceItem
                {
                    Date = s.Date,
                    Vehicle = s.Vehicle?.Registration ?? "-",
                    Description = s.Description,
                    Cost = s.Cost,
                }).ToList()
            };

            return Page();
        }
    }
}
