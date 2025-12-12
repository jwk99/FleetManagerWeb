using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FleetManagerWeb.Data;
using Microsoft.EntityFrameworkCore;
using FleetManagerWeb.Models;

namespace FleetManagerWeb.Pages.Vacations
{
    public class IndexModel : PageModel
    {
        private readonly FleetDbContext _db;
        public IndexModel(FleetDbContext db)
        {
            _db = db;
        }

        public IList<Vacation> Vacations { get; set; }

        public async Task OnGetAsync()
        {
            Vacations = await _db.Vacations
                .Include(v => v.Driver)
                .Include(v => v.SubstituteDriver)
                .OrderByDescending(v => v.From)
                .ToListAsync();
        }
    }
}
