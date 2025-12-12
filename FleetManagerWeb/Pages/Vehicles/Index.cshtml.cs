using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FleetManagerWeb.Data;
using FleetManagerWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace FleetManagerWeb.Pages.Vehicles
{
    public class IndexModel : PageModel
    {
        private readonly FleetDbContext _db;

        public IndexModel(FleetDbContext db)
        {
            _db = db;
        }

        public List<Vehicle> Vehicles { get; set; }

        public async Task OnGetAsync()
        {
            Vehicles = await _db.Vehicles.ToListAsync();
        }
    }
}
