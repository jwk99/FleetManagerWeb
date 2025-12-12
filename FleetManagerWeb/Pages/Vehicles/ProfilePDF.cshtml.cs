using FleetManagerWeb.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace FleetManagerWeb.Pages.Vehicles
{
    public class ProfilePDFModel : PageModel
    {
        private readonly FleetDbContext _db;
        public ProfilePDFModel(FleetDbContext db) => _db = db;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var vehicle = await _db.Vehicles.FirstOrDefaultAsync(v => v.Id == id);
            if (vehicle == null) return NotFound();

            var services = await _db.ServiceRecords
                .Where(s => s.VehicleId == id)
                .OrderByDescending(s => s.Date)
                .ToListAsync();
            QuestPDF.Settings.License = LicenseType.Community;

            var pdf = Document.Create(document =>
            {
                document.Page(page =>
                {
                    page.Margin(30);
                    page.Header()
                        .Text($"Raport pojazdu – {vehicle.Registration}")
                        .FontSize(20)
                        .Bold();

                    page.Content().Column(col =>
                    {
                        col.Item().Text($"Marka: {vehicle.Brand}");
                        col.Item().Text($"Model: {vehicle.Model}");
                        col.Item().Text($"Rok: {vehicle.Year}");
                        col.Item().Text($"VIN: {vehicle.VIN}");
                        col.Item().Text($"Przebieg: {vehicle.Mileage} km");

                        col.Item().PaddingTop(15).Text("Historia serwisów").FontSize(16).Bold();

                        foreach (var s in services)
                        {
                            col.Item().Text($"{s.Date.ToShortDateString()} – {s.Description} – {s.Cost} z³");
                        }
                    });

                    page.Footer()
                        .AlignCenter()
                        .Text(x =>
                        {
                            x.Span("FleetManager – ").Bold();
                            x.Span(DateTime.Now.ToString("dd.MM.yyyy HH:mm"));
                        });
                });
            });

            var file = pdf.GeneratePdf();
            return File(file, "application/pdf", $"Vehicle_{vehicle.Registration}.pdf");
        }
    }
}
