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
                .FirstOrDefaultAsync(m => m.Id == id);
            if (computers == null)
            {
                return NotFound();
            }

            return View(computers);
        }

        // GET: Computers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Computers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Brand")] Computers computers)
        {
            if (ModelState.IsValid)
            {
                _context.Add(computers);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(computers);
        }

        // GET: Computers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Computers == null)
            {
                return NotFound();
            }

            var computers = await _context.Computers.FindAsync(id);
            if (computers == null)
            {
                return NotFound();
            }
            return View(computers);
        }

        // POST: Computers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Brand")] Computers computers)
        {
            if (id != computers.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(computers);
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
            return View(computers);
        }

        // GET: Computers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Computers == null)
            {
                return NotFound();
            }

            var computers = await _context.Computers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (computers == null)
            {
                return NotFound();
            }

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
