using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FleetManagerWeb.Data;
using Microsoft.EntityFrameworkCore;

namespace FleetManagerWeb.Pages.Vehicles
{
    public class ProfileModel : PageModel
    {
        private readonly FleetDbContext _db;
        public ProfileModel(FleetDbContext db)
        {
            _db = db;
        }
        public class VehicleProfileVm
        {
            public int Id { get; set; }
            public string Brand { get; set; }
            public string Model {  get; set; }
            public string Registration {  get; set; }
            public string VIN { get; set; }
            public int Year { get; set; }
            public int Mileage { get; set; }

            public decimal TotalCost {  get; set; }
            public int TotalServices { get; set; }
            public DateTime? LastServiceDate { get; set; }
            public DateTime? NextPlannedService {  get; set; }

            public List<ServiceItem> Services { get; set; }
        }
        public class ServiceItem
        {
            public DateTime Date { get; set; }
            public string Description { get; set; }
            public decimal Cost { get; set; }
            public bool IsCompleted { get; set; }
        }
        public VehicleProfileVm Vehicle {  get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var vehicle = await _db.Vehicles.FirstOrDefaultAsync(v=>v.Id == id);
            if (vehicle == null)
                return NotFound();
            var services = await _db.ServiceRecords
                .Where(s=>s.VehicleId == id)
                .OrderByDescending(s=>s.Date)
                .ToListAsync();
            Vehicle = new VehicleProfileVm
            {
                Id = vehicle.Id,
                Brand = vehicle.Brand,
                Model = vehicle.Model,
                Registration = vehicle.Registration,
                VIN = vehicle.VIN,
                Year = vehicle.Year,
                Mileage = vehicle.Mileage,

                TotalCost = services.Sum(s => s.Cost),
                TotalServices = services.Count,
                LastServiceDate = services.Where(s => s.IsCompleted).Max(s => (DateTime?)s.CompletedDate),
                NextPlannedService = services.Where(s => !s.IsCompleted).Min(s => (DateTime?)s.Date),

                Services = services.Select(s => new ServiceItem
                {
                    Date = s.Date,
                    Description = s.Description,
                    Cost = s.Cost,
                    IsCompleted = s.IsCompleted,
                }).ToList()
            };

            return Page();
        }
    }
}
