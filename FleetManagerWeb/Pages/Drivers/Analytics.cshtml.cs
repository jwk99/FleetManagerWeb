using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FleetManagerWeb.Data;
using FleetManagerWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace FleetManagerWeb.Pages.Drivers
{
    public class AnalyticsModel : PageModel
    {
        private readonly FleetDbContext _db;

        public AnalyticsModel(FleetDbContext db)
        {
            _db = db;
        }

        public class DriverStats
        {
            public int DriverId { get; set; }
            public String Name { get; set; }
            public int ServiceCount { get; set; }
            public decimal TotalCost { get; set; }
            public decimal AverageCost { get; set; }
        }
        public List<DriverStats> Stats { get; set; }

        public int TotalDrivers { get; set; }
        public int TotalServices { get; set; }
        public decimal TotalCostAll { get; set; }
        public decimal AverageCostAll { get; set; }

        public async Task OnGetAsync()
        {
            var drivers = await _db.Drivers.ToListAsync();
            var services = await _db.ServiceRecords.ToListAsync();

            Stats = drivers.
                GroupJoin(
                services,
                d => d.Id,
                s => s.DriverId,
                (d, s) => new DriverStats
                {
                    DriverId = d.Id,
                    Name = d.LastName,
                    ServiceCount = s.Count(),
                    TotalCost = s.Sum(x => x.Cost),
                    AverageCost = s.Any() ? s.Average(x => x.Cost) : 0
                }).ToList();
            TotalDrivers = drivers.Count;
            TotalServices = services.Count(s => s.DriverId != null);
            TotalCostAll = services.Sum(s => s.Cost);
            AverageCostAll = services.Any() ? services.Average(s => s.Cost) : 0;
        }
    }
}
