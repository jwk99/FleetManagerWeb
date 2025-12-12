using FleetManagerWeb.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace FleetManagerWeb.Pages.Vehicles
{
    public class AnalyticsModel : PageModel
    {
        private readonly FleetDbContext _db;
        public AnalyticsModel(FleetDbContext db) => _db = db;

        public class VehicleAnalyticsItem
        {
            public int VehicleId { get; set; }
            public string Registration { get; set; }
            public decimal TotalServiceCost { get; set; }
            public int ServiceCount { get; set; }

            // RISK MODEL
            public int RiskScore { get; set; }
            public string RiskLevel { get; set; }
            public decimal Probability { get; set; }
        }

        public List<VehicleAnalyticsItem> Analytics { get; set; }

        public async Task OnGetAsync()
        {
            var vehicles = await _db.Vehicles.ToListAsync();
            var services = await _db.ServiceRecords.ToListAsync();

            Analytics = new List<VehicleAnalyticsItem>();

            foreach (var v in vehicles)
            {
                var vServices = services.Where(s => s.VehicleId == v.Id).ToList();
                var completed = vServices.Where(s => s.IsCompleted).ToList();

                decimal totalCost = vServices.Sum(s => s.Cost);
                int count = vServices.Count;

                // ------ RISK SCORE ------
                int score = 0;

                int age = DateTime.Now.Year - v.Year;
                if (age >= 10) score += 4;
                else if (age >= 5) score += 2;

                if (v.Mileage >= 250000) score += 4;
                else if (v.Mileage >= 150000) score += 2;

                if (count >= 5) score += 2;

                if (count > 0 && vServices.Average(s => s.Cost) >= 1500)
                    score += 2;

                var lastServ = completed
                    .Where(s => s.CompletedDate != null)
                    .OrderByDescending(s => s.CompletedDate)
                    .FirstOrDefault();

                if (lastServ != null)
                {
                    var diff = DateTime.Now - lastServ.CompletedDate.Value;
                    if (diff.TotalDays > 180) score += 3;
                    else if (diff.TotalDays > 90) score += 1;
                }

                string level =
                    score <= 4 ? "Niskie" :
                    score <= 8 ? "Œrednie" :
                    "Wysokie";

                Analytics.Add(new VehicleAnalyticsItem
                {
                    VehicleId = v.Id,
                    Registration = v.Registration,
                    TotalServiceCost = totalCost,
                    ServiceCount = count,
                    RiskScore = score,
                    RiskLevel = level,
                    Probability = Math.Round((decimal)score / 15m * 100m, 1)
                });
            }
        }
    }
}
