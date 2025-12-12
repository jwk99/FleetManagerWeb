using FleetManagerWeb.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace FleetManagerWeb.Pages.Drivers
{
    public class ProfilePDFModel : PageModel
    {
        private readonly FleetDbContext _db;
        public ProfilePDFModel(FleetDbContext db) => _db = db;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var driver = await _db.Drivers.FirstOrDefaultAsync(d => d.Id == id);
            if (driver == null) return NotFound();

            var services = await _db.ServiceRecords
                .Where(s => s.DriverId == id)
                .Include(s => s.Vehicle)
                .OrderByDescending(s => s.Date)
                .ToListAsync();
            QuestPDF.Settings.License = LicenseType.Community;

            var pdf = Document.Create(doc =>
            {
                doc.Page(page =>
                {
                    page.Margin(30);

                    page.Header().Text($"Raport kierowcy – {driver.FirstName} {driver.LastName}")
                        .FontSize(20)
                        .Bold();

                    page.Content().Column(col =>
                    {
                        col.Item().Text($"Telefon: {driver.Phone}");
                        col.Item().Text($"Numer prawa jazdy: {driver.LicenseNumber}");

                        col.Item().PaddingTop(15).Text("Historia serwisów").FontSize(16).Bold();

                        foreach (var s in services)
                        {
                            col.Item().Text(
                                $"{s.Date.ToShortDateString()} – {s.Description} – {s.Cost} z³ – {s.Vehicle?.Registration}"
                            );
                        }
                    });

                    page.Footer()
                        .AlignCenter()
                        .Text(DateTime.Now.ToString("dd.MM.yyyy HH:mm"));
                });
            });

            var file = pdf.GeneratePdf();
            return File(file, "application/pdf", $"Driver_{driver.LastName}.pdf");
        }
    }
}
