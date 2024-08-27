using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Booking_clothes.Data;
using Booking_clothes.Models;

namespace Booking_clothes.Controllers
{
    public class BancksController : Controller
    {
        private readonly MyContext _context;

        public BancksController(MyContext context)
        {
            _context = context;
        }

        // GET: Bancks
        public async Task<IActionResult> Index()
        {
              return _context.Banck != null ? 
                          View(await _context.Banck.ToListAsync()) :
                          Problem("Entity set 'MyContext.Banck'  is null.");
        }

        // GET: Bancks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Banck == null)
            {
                return NotFound();
            }

            var banck = await _context.Banck
                .FirstOrDefaultAsync(m => m.Id == id);
            if (banck == null)
            {
                return NotFound();
            }

            return View(banck);
        }

        // GET: Bancks/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Bancks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CVV,CardHolder,CardNumber,ExpiryDate,Balance")] Banck banck)
        {
           
                _context.Add(banck);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            
            return View(banck);
        }

        // GET: Bancks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Banck == null)
            {
                return NotFound();
            }

            var banck = await _context.Banck.FindAsync(id);
            if (banck == null)
            {
                return NotFound();
            }
            return View(banck);
        }

        // POST: Bancks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CVV,CardHolder,CardNumber,ExpiryDate,Balance")] Banck banck)
        {
            if (id != banck.Id)
            {
                return NotFound();
            }

           
                try
                {
                    _context.Update(banck);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BanckExists(banck.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            
            return View(banck);
        }

        // GET: Bancks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Banck == null)
            {
                return NotFound();
            }

            var banck = await _context.Banck
                .FirstOrDefaultAsync(m => m.Id == id);
            if (banck == null)
            {
                return NotFound();
            }

            return View(banck);
        }

        // POST: Bancks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Banck == null)
            {
                return Problem("Entity set 'MyContext.Banck'  is null.");
            }
            var banck = await _context.Banck.FindAsync(id);
            if (banck != null)
            {
                _context.Banck.Remove(banck);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BanckExists(int id)
        {
          return (_context.Banck?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
