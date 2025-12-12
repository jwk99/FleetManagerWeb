using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FleetManagerWeb.Data;
using FleetManagerWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace FleetManagerWeb.Pages.ServiceRecords
{
    public class DeleteModel : PageModel
    {
        private readonly FleetDbContext _db;
        public DeleteModel(FleetDbContext db)
        {
            _db = db;
        }
        [BindProperty]
        public ServiceRecord Record { get; set; }
        public async Task<IActionResult> OnGetAsync(int id)
        {
            Record = await _db.ServiceRecords
                .Include(s => s.Vehicle)
                .Include(s => s.Driver)
                .FirstOrDefaultAsync(s => s.Id == id);
            if(Record == null)
                return NotFound();
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            var record=await _db.ServiceRecords.FindAsync(Record.Id);
            if (record != null)
            {
                _db.ServiceRecords.Remove(record);
                await _db.SaveChangesAsync();
            }
            return RedirectToPage("Index");
        }
    }
}
