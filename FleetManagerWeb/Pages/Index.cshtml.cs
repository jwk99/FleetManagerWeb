using FleetManagerWeb.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace FleetManagerWeb.Pages
{
    public class IndexModel : PageModel
    {
        private readonly FleetDbContext _db;
        public IndexModel(FleetDbContext db)
        {
            _db = db;
        }

        public int VehicleCount { get; set; }
        public int DriverCount { get; set; }
        public decimal ServiceCostYear { get; set; }
        public List<decimal> MonthlyCosts { get; set; }
        public int OverdueServices { get; set; }
        public double AverageVehicleAge { get; set; }
        public double VehiclesPerDriver {  get; set; }

        public async Task OnGetAsync()
        {
            VehicleCount = await _db.Vehicles.CountAsync();
            DriverCount = await _db.Drivers.CountAsync();
            var vehicles = await _db.Vehicles.ToListAsync();

            var year = DateTime.Now.Year;

            var records = await _db.ServiceRecords
                .Where(r => r.Date.Year == year)
                .ToListAsync();

            ServiceCostYear = records.Sum(r => r.Cost);

            MonthlyCosts = Enumerable.Range(1, 12)
                .Select(m => records
                    .Where(r => r.Date.Month == m)
                    .Sum(r => r.Cost))
                .ToList();

            OverdueServices = _db.ServiceRecords
                .Where(s => s.Date < DateTime.Now && !s.IsCompleted)
                .Count();


            VehiclesPerDriver = DriverCount > 0
                ? Math.Round((double)VehicleCount / DriverCount, 2)
                : 0;

            if (vehicles.Count > 0)
            {
                AverageVehicleAge = vehicles
                    .Where(v=>v.Year > 0)
                    .Average(v=>DateTime.Now.Year - v.Year);
            }
            else
            {
                AverageVehicleAge = 0;
            }
        }
    }
}
