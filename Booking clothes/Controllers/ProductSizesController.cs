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
    public class ProductSizesController : Controller
    {
        private readonly MyContext _context;

        public ProductSizesController(MyContext context)
        {
            _context = context;
        }

        // GET: ProductSizes
        public async Task<IActionResult> Index()
        {
            var myContext = _context.ProductSize.Include(p => p.Products).Include(p => p.Size);
            return View(await myContext.ToListAsync());
        }

        // GET: ProductSizes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ProductSize == null)
            {
                return NotFound();
            }

            var productSize = await _context.ProductSize
                .Include(p => p.Products)
                .Include(p => p.Size)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productSize == null)
            {
                return NotFound();
            }

            return View(productSize);
        }

        // GET: ProductSizes/Create
        public IActionResult Create()
        {
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name");
            ViewData["SizeID"] = new SelectList(_context.Sizes, "Id", "SizeName");
            return View();
        }

        // POST: ProductSizes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Quantity,SizeID,ProductId")] ProductSize productSize)
        {
            productSize.Quantity = 1;
                _context.Add(productSize);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name", productSize.ProductId);
            ViewData["SizeID"] = new SelectList(_context.Sizes, "Id", "SizeName", productSize.SizeID);
            return View(productSize);
        }

        // GET: ProductSizes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ProductSize == null)
            {
                return NotFound();
            }

            var productSize = await _context.ProductSize.FindAsync(id);
            if (productSize == null)
            {
                return NotFound();
            }
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name", productSize.ProductId);
            ViewData["SizeID"] = new SelectList(_context.Sizes, "Id", "SizeName", productSize.SizeID);
            return View(productSize);
        }

        // POST: ProductSizes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Quantity,SizeID,ProductId")] ProductSize productSize)
        {
            if (id != productSize.Id)
            {
                return NotFound();
            }

            
                try
                {
                    _context.Update(productSize);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductSizeExists(productSize.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name", productSize.ProductId);
            ViewData["SizeID"] = new SelectList(_context.Sizes, "Id", "SizeName", productSize.SizeID);
            return View(productSize);
        }

        // GET: ProductSizes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ProductSize == null)
            {
                return NotFound();
            }

            var productSize = await _context.ProductSize
                .Include(p => p.Products)
                .Include(p => p.Size)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productSize == null)
            {
                return NotFound();
            }

            return View(productSize);
        }

        // POST: ProductSizes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ProductSize == null)
            {
                return Problem("Entity set 'MyContext.ProductSize'  is null.");
            }
            var productSize = await _context.ProductSize.FindAsync(id);
            if (productSize != null)
            {
                _context.ProductSize.Remove(productSize);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductSizeExists(int id)
        {
          return (_context.ProductSize?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
