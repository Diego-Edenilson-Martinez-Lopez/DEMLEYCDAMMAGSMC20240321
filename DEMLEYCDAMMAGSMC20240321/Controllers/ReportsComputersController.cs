using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using Rotativa.AspNetCore;
using DEMLEYCDAMMAGSMC20240321.Models;
using System.Linq;
using System.Threading.Tasks;

namespace DEMLEYCDAMMAGSMC20240321.Controllers
{
    public class ReportsComputersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReportsComputersController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GenerarReporte(string nombre, string marca, string opcion)
        {
            var query = _context.Computers.AsQueryable();

            if (!string.IsNullOrEmpty(nombre))
            {
                query = query.Where(c => c.Name.Contains(nombre));
            }

            if (!string.IsNullOrEmpty(marca))
            {
                query = query.Where(c => c.Brand.Contains(marca));
            }

            var computadoras = await query.Include(c => c.Components).ToListAsync();

            if (opcion == "PDF")
            {
                return new ViewAsPdf("ReportsComputers", computadoras)
                {
                    // ...
                };
            }
            else if (opcion == "EXCEL")
            {
                using (var package = new ExcelPackage())
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Computadoras");
                    worksheet.Cells["A1"].Value = "Nombre";
                    worksheet.Cells["B1"].Value = "Marca";
                    worksheet.Cells["C1"].Value = "Descripción de Componentes";

                    int row = 2;
                    foreach (var computadora in computadoras)
                    {
                        worksheet.Cells[row, 1].Value = computadora.Name;
                        worksheet.Cells[row, 2].Value = computadora.Brand;
                        worksheet.Cells[row, 3].Value = string.Join(", ", computadora.Components.Select(c => c.Description));
                        row++;
                    }

                    var range = worksheet.Cells["A1:C" + row];
                    range.AutoFilter = true;

                    byte[] fileContents = package.GetAsByteArray();

                    return File(fileContents, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "computadoras.xlsx");
                }
            }
            else
            {
                return Content("Opción no válida");
            }
        }
    }
}
