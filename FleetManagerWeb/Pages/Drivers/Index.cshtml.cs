using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FleetManagerWeb.Models;
using FleetManagerWeb.Data;

namespace FleetManagerWeb.Pages.Drivers
{
    public class IndexModel : PageModel
    {
        private readonly FleetDbContext _db;

        public IndexModel(FleetDbContext db)
        {
            _db = db;
        }

        public IList<Driver> Drivers { get; set; }
        public async Task OnGetAsync()
        {
            Drivers= await _db.Drivers.ToListAsync();
        }
    }
}
