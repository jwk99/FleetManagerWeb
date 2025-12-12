using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FleetManagerWeb.Data;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using SkiaSharp;

namespace FleetManagerWeb.Pages
{
    public class ExportToPDFModel : PageModel
    {
        private readonly FleetDbContext _db;
        public ExportToPDFModel(FleetDbContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> OnGetAsync()
        {
            var vehicles = await _db.Vehicles.ToListAsync();
            var services = await _db.ServiceRecords.Include(s => s.Vehicle).ToListAsync();
            var vehicleTable = vehicles.Select(v => new
            {
                v.Brand,
                v.Model,
                v.Registration,
                v.Year,
                Count = services.Count(s => s.VehicleId == v.Id),
                Cost = services.Where(s => s.VehicleId == v.Id).Sum(s => s.Cost)
            }).ToList();
            var monthlyCost = Enumerable.Range(1,12)
                .Select(m=>services.Where(s=>s.Date.Month==m).Sum(s=>s.Cost)).ToList();
            var monthlyCount = Enumerable.Range(1,12)
                .Select(m=>services.Count(s=>s.Date.Month==m)).ToList();
            byte[] GenerateBarChart(string title, IList<float> values)
            {
                int width = 900;
                int height = 500;
                using var surface = SKSurface.Create(new SKImageInfo(width, height));
                var canvas = surface.Canvas;
                canvas.Clear(SKColors.White);
                var titlePaint = new SKPaint
                {
                    Color = SKColors.Black,
                    TextSize = 28,
                    IsAntialias = true,
                    Typeface = SKTypeface.FromFamilyName("Arial", SKFontStyle.Bold)
                };
                var axisPaint = new SKPaint
                {
                    Color = SKColors.Black,
                    StrokeWidth = 2,
                    IsAntialias = true
                };
                var gridPaint = new SKPaint
                {
                    Color = SKColors.LightGray,
                    StrokeWidth = 1,
                    IsAntialias = true
                };
                var labelPaint = new SKPaint
                {
                    Color = SKColors.Black,
                    TextSize = 18,
                    IsAntialias = true
                };
                var valuePaint = new SKPaint
                {
                    Color = SKColors.Black,
                    TextSize = 16,
                    IsAntialias = true
                };
                canvas.DrawText(title, 20, 40, titlePaint);
                int chartLeft = 70;
                int chartBottom = height - 60;
                int chartTop = 80;
                int chartRight = width - 30;
                float max = values.Max();
                if (max == 0) max = 1;
                int gridLines = 5;
                for (int i = 0; i <= gridLines; i++)
                {
                    float y = chartBottom - (i * (chartBottom - chartTop) / gridLines);
                    canvas.DrawLine(chartLeft, y, chartRight, y, gridPaint);
                    float labelValue = max * i / gridLines;
                    canvas.DrawText(labelValue.ToString("0"), 20, y + 5, labelPaint);
                }
                canvas.DrawLine(chartLeft, chartTop, chartLeft, chartBottom, axisPaint);   // Y
                canvas.DrawLine(chartLeft, chartBottom, chartRight, chartBottom, axisPaint); // X
                int count = values.Count;
                int barWidth = (chartRight - chartLeft - (count * 10)) / count;
                for (int i = 0; i < count; i++)
                {
                    float value = values[i];
                    float heightFactor = value / max;
                    float barHeight = heightFactor * (chartBottom - chartTop);
                    float x = chartLeft + i * (barWidth + 10);
                    float y = chartBottom - barHeight;
                    var barPaint = new SKPaint { Color = SKColors.SteelBlue };
                    canvas.DrawRect(x, y, barWidth, barHeight, barPaint);
                    canvas.DrawText(value.ToString("0"), x + barWidth / 4, y - 5, valuePaint);
                    canvas.DrawText($"M{i + 1}", x + barWidth / 4, chartBottom + 20, labelPaint);
                }
                return surface.Snapshot().Encode().ToArray();
            }

            var monthlyCostImg = GenerateBarChart("Koszty miesiêczne", monthlyCost.Select(c => (float)c).ToList());
            var monthlyCountImg = GenerateBarChart("Liczba serwisów miesiêcznie", monthlyCount.Select(c => (float)c).ToList());
            QuestPDF.Settings.License = LicenseType.Community;
            var pdf = Document.Create(doc =>
            {
                doc.Page(page =>
                {
                    page.Margin(30);
                    page.Header()
                    .Text("Raport Analizy Floty")
                    .FontSize(22)
                    .Bold()
                    .AlignCenter();
                    page.Content().Column(col =>
                    {
                        col.Item().PaddingBottom(10).Text("Podsumowanie").FontSize(16).Bold();
                        col.Item().Text($"Œredni wiek pojazdu: {vehicles.Average(v => DateTime.Now.Year - v.Year):0.0} lat");
                        col.Item().Text($"Œredni koszt serwisu: {services.Average(s => s.Cost):0.0} z³");
                        col.Item().Text($"Najdro¿szy pojazd: {vehicleTable.OrderByDescending(v => v.Cost).First().Registration} - {vehicleTable.Max(v => v.Cost)} z³");
                        col.Item().PaddingTop(20).Text("Tabela pojazdów").FontSize(16).Bold();
                        col.Item().Table(t =>
                        {
                            t.ColumnsDefinition(c =>
                            {
                                c.ConstantColumn(80);
                                c.ConstantColumn(80);
                                c.RelativeColumn();
                                c.ConstantColumn(60);
                                c.ConstantColumn(80);
                                c.ConstantColumn(80);
                            });
                            t.Header(h =>
                            {
                                h.Cell().Text("Marka").Bold();
                                h.Cell().Text("Model").Bold();
                                h.Cell().Text("Rejestracja").Bold();
                                h.Cell().Text("Rok").Bold();
                                h.Cell().Text("Serwisy").Bold();
                                h.Cell().Text("Koszt").Bold();
                            });
                            foreach (var v in vehicleTable)
                            {
                                t.Cell().Text(v.Brand);
                                t.Cell().Text(v.Model);
                                t.Cell().Text(v.Registration);
                                t.Cell().Text(v.Year.ToString());
                                t.Cell().Text(v.Count.ToString());
                                t.Cell().Text($"{v.Cost} z³");
                            }
                        });
                        col.Item().Image(monthlyCostImg);
                        col.Item().Image(monthlyCountImg);
                    });
                    page.Footer().AlignRight().Text(DateTime.Now.ToString("dd.MM.yyyy HH:mm"));
                });
            });
            var file = pdf.GeneratePdf();
            return File(file, "application/pdf", "Fleet_Analytics_Report.pdf");
        }  
    }
}
