using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FleetManagerWeb.Data;
using FleetManagerWeb.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
namespace FleetManagerWeb.Pages.ServiceRecords
{
    public class EditModel : PageModel
    {
        private readonly FleetDbContext _db;
        public EditModel(FleetDbContext db)
        {  _db = db; }
        [BindProperty]
        public ServiceRecord Record { get; set; }
        public SelectList Vehicles { get; set; }
        public SelectList Drivers { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Record = await _db.ServiceRecords.FindAsync(id);
            if(Record == null)
                return NotFound();
            Vehicles=new SelectList(_db.Vehicles.ToList(),"Id","Registration");
            Drivers = new SelectList(_db.Drivers.ToList(), "Id", "LastName");
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            Vehicles = new SelectList(_db.Vehicles.ToList(), "Id", "Registration");
            Drivers = new SelectList(_db.Drivers.ToList(), "Id", "LastName");

            if (!ModelState.IsValid)
                return Page();

            var recordInDb = await _db.ServiceRecords.FindAsync(Record.Id);

            if (recordInDb == null)
                return NotFound();

            recordInDb.VehicleId = Record.VehicleId;
            recordInDb.DriverId = Record.DriverId;
            recordInDb.Date = Record.Date;
            recordInDb.Description = Record.Description;
            recordInDb.Cost = Record.Cost;
            recordInDb.IsCompleted = Record.IsCompleted;
            recordInDb.CompletedDate = Record.CompletedDate;

            await _db.SaveChangesAsync();

            return RedirectToPage("Index");
        }
    }
}
