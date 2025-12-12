using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FleetManagerWeb.Data;
using FleetManagerWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace FleetManagerWeb.Pages.ServiceRecords
{
    public class IndexModel : PageModel
    {
        private readonly FleetDbContext _db;
        public IndexModel(FleetDbContext db)
        {
            _db = db;
        }
        public IList<ServiceRecord> Records { get; set; }
        public async Task OnGetAsync()
        {
            Records = await _db.ServiceRecords
                .Include(s => s.Vehicle)
                .Include(s => s.Driver)
                .ToListAsync();
        }
    }
}
