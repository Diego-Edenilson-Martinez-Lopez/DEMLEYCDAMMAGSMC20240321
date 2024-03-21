using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DEMLEYCDAMMAGSMC20240321.Models;

namespace DEMLEYCDAMMAGSMC20240321.Controllers
{
    public class ComputersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ComputersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Computers
        public async Task<IActionResult> Index()
        {
              return _context.Computers != null ? 
                          View(await _context.Computers.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Computers'  is null.");
        }

        // GET: Computers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Computers == null)
            {
                return NotFound();
            }

            var computers = await _context.Computers
                .Include(s=> s.Components)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (computers == null)
            {
                return NotFound();
            }
            ViewBag.Accion = "Details";
            return View(computers);
        }

        // GET: Computers/Create
        public IActionResult Create()
        {
            var Computers = new Computers();
            Computers.Components = new List<Components>();
            Computers.Components.Add(new Components
            {

            });
            ViewBag.Accion = "Create";
            return View(Computers);
        }

        // POST: Computers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Brand,Components")] Computers computers)
        {
            _context.Add(computers);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
            //return View(computers);
        }
        public ActionResult AgregarDetalles([Bind("Id,Name,Brand,Components")] Computers computers, string accion)
        {
            computers.Components.Add(new Components { });
            ViewBag.Accion = accion;
            return View(accion, computers);
        }
        public ActionResult EliminarDetalles([Bind("Id,Name,Brand,Components")] Computers computers, int index, string accion)
        {
            var det = computers.Components[index];
            if (accion == "Edit" && det.Id > 0)
            {
                det.Id = det.Id * -1;
            }
            else
            {
                computers.Components.RemoveAt(index);
            }

            ViewBag.Accion = accion;
            return View(accion, computers);
        }

        // GET: Computers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Computers == null)
            {
                return NotFound();
            }

            var computers = await _context.Computers
                .Include(s=> s.Components)
                .FirstAsync(s=> s.Id==id);
            if (computers == null)
            {
                return NotFound();
            }
            ViewBag.Accion = "Edit";
            return View(computers);
        }

        // POST: Computers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Brand,Components")] Computers computers)
        {
            if (id != computers.Id)
            {
                return NotFound();
            }

            try
            {
                // Obtener los datos de la base de datos que van a ser modificados
                var facturaUpdate = await _context.Computers
                        .Include(s => s.Components)
                        .FirstAsync(s => s.Id == computers.Id);
                facturaUpdate.Name = computers.Name;
                facturaUpdate.Brand = computers.Brand;
                // Obtener todos los detalles que seran nuevos y agregarlos a la base de datos
                var detNew = computers.Components.Where(s => s.Id == 0);
                foreach (var d in detNew)
                {
                    facturaUpdate.Components.Add(d);
                }
                // Obtener todos los detalles que seran modificados y actualizar a la base de datos
                var detUpdate = computers.Components.Where(s => s.Id > 0);
                foreach (var d in detUpdate)
                {
                    var det = facturaUpdate.Components.FirstOrDefault(s => s.Id == d.Id);
                    det.Name = d.Name;
                    det.Type = d.Type;
                    det.Description = d.Description;
                }
                // Obtener todos los detalles que seran eliminados y actualizar a la base de datos
                var delDet = computers.Components.Where(s => s.Id < 0).ToList();
                if (delDet != null && delDet.Count > 0)
                {
                    foreach (var d in delDet)
                    {
                        d.Id = d.Id * -1;
                        var det = facturaUpdate.Components.FirstOrDefault(s => s.Id == d.Id);
                        _context.Remove(det);
                        // facturaUpdate.DetFacturaVenta.Remove(det);
                    }
                }
                // Aplicar esos cambios a la base de datos
                _context.Update(facturaUpdate);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ComputersExists(computers.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Computers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Computers == null)
            {
                return NotFound();
            }

            var computers = await _context.Computers
                .Include(s=> s.Components)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (computers == null)
            {
                return NotFound();
            }
            ViewBag.Accion = "Delete";
            return View(computers);
        }

        // POST: Computers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Computers == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Computers'  is null.");
            }
            var computers = await _context.Computers.FindAsync(id);
            if (computers != null)
            {
                _context.Computers.Remove(computers);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ComputersExists(int id)
        {
          return (_context.Computers?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
