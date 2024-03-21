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
    public class ComponentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ComponentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Components
        public async Task<IActionResult> Index()
        {
              return _context.Components != null ? 
                          View(await _context.Components.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Components'  is null.");
        }

        // GET: Components/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Components == null)
            {
                return NotFound();
            }

            var components = await _context.Components
                .FirstOrDefaultAsync(m => m.Id == id);
            if (components == null)
            {
                return NotFound();
            }

            return View(components);
        }

        // GET: Components/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Components/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ComputerId,Name,Type,Description")] Components components)
        {
            if (ModelState.IsValid)
            {
                _context.Add(components);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(components);
        }

        // GET: Components/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Components == null)
            {
                return NotFound();
            }

            var components = await _context.Components.FindAsync(id);
            if (components == null)
            {
                return NotFound();
            }
            return View(components);
        }

        // POST: Components/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ComputerId,Name,Type,Description")] Components components)
        {
            if (id != components.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(components);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ComponentsExists(components.Id))
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
            return View(components);
        }

        // GET: Components/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Components == null)
            {
                return NotFound();
            }

            var components = await _context.Components
                .FirstOrDefaultAsync(m => m.Id == id);
            if (components == null)
            {
                return NotFound();
            }

            return View(components);
        }

        // POST: Components/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Components == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Components'  is null.");
            }
            var components = await _context.Components.FindAsync(id);
            if (components != null)
            {
                _context.Components.Remove(components);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ComponentsExists(int id)
        {
          return (_context.Components?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
