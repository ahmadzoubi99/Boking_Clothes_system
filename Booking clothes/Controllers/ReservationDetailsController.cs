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
    public class ReservationDetailsController : Controller
    {
        private readonly MyContext _context;

        public ReservationDetailsController(MyContext context)
        {
            _context = context;
        }

        // GET: ReservationDetails
        public async Task<IActionResult> Index()
        {
            var myContext = _context.ReservationDetails.Include(r => r.Reservation).Include(r => r.products);
            return View(await myContext.ToListAsync());
        }

        // GET: ReservationDetails/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ReservationDetails == null)
            {
                return NotFound();
            }

            var reservationDetail = await _context.ReservationDetails
                .Include(r => r.Reservation)
                .Include(r => r.products)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reservationDetail == null)
            {
                return NotFound();
            }

            return View(reservationDetail);
        }

        // GET: ReservationDetails/Create
        public IActionResult Create()
        {
            ViewData["ReservationId"] = new SelectList(_context.Reservations, "Id", "Status");
            ViewData["ClothId"] = new SelectList(_context.Products, "Id", "Name");
            return View();
        }

        // POST: ReservationDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ReservationId,ClothId,Quantity,StartReservationDate,EndReservationDate")] ReservationDetail reservationDetail)
        {
            
                _context.Add(reservationDetail);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            
            ViewData["ReservationId"] = new SelectList(_context.Reservations, "Id", "Status", reservationDetail.ReservationId);
            ViewData["ClothId"] = new SelectList(_context.Products, "Id", "Name", reservationDetail.ClothId);
            return View(reservationDetail);
        }

        // GET: ReservationDetails/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ReservationDetails == null)
            {
                return NotFound();
            }

            var reservationDetail = await _context.ReservationDetails.FindAsync(id);
            if (reservationDetail == null)
            {
                return NotFound();
            }
            ViewData["ReservationId"] = new SelectList(_context.Reservations, "Id", "Status", reservationDetail.ReservationId);
            ViewData["ClothId"] = new SelectList(_context.Products, "Id", "Name", reservationDetail.ClothId);
            return View(reservationDetail);
        }

        // POST: ReservationDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ReservationId,ClothId,Quantity,StartReservationDate,EndReservationDate")] ReservationDetail reservationDetail)
        {
            if (id != reservationDetail.Id)
            {
                return NotFound();
            }

            
                try
                {
                    _context.Update(reservationDetail);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservationDetailExists(reservationDetail.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            
            ViewData["ReservationId"] = new SelectList(_context.Reservations, "Id", "Status", reservationDetail.ReservationId);
            ViewData["ClothId"] = new SelectList(_context.Products, "Id", "Name", reservationDetail.ClothId);
            return View(reservationDetail);
        }

        // GET: ReservationDetails/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ReservationDetails == null)
            {
                return NotFound();
            }

            var reservationDetail = await _context.ReservationDetails
                .Include(r => r.Reservation)
                .Include(r => r.products)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reservationDetail == null)
            {
                return NotFound();
            }

            return View(reservationDetail);
        }

        // POST: ReservationDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ReservationDetails == null)
            {
                return Problem("Entity set 'MyContext.ReservationDetails'  is null.");
            }
            var reservationDetail = await _context.ReservationDetails.FindAsync(id);
            if (reservationDetail != null)
            {
                _context.ReservationDetails.Remove(reservationDetail);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReservationDetailExists(int id)
        {
          return (_context.ReservationDetails?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
