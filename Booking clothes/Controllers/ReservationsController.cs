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
    public class ReservationsController : Controller
    {
        private readonly MyContext _context;

        public ReservationsController(MyContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var myContext = _context.Reservations.Include(r => r.User).Include(r => r.Address);
            return View(await myContext.ToListAsync());
        }

        public async Task<IActionResult> ReservationDetails(int id)
        {
            var myContext = _context.ReservationDetails.Include(o => o.Reservation).Include(o => o.products).Where(p => p.ReservationId == id);
            var Reservation = _context.Reservations.Where(p => p.Id == id).Include(u=>u.User).FirstOrDefault();
            ViewBag.TotalAmount = Reservation.TotalAmount;
            ViewBag.userName = Reservation.User.FirstName+" "+ Reservation.User.LastName;

            return View(await myContext.ToListAsync());
        }


        // GET: Reservations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Reservations == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations
                .Include(r => r.Address)
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // GET: Reservations/Create
        public IActionResult Create()
        {
            ViewData["AddressId"] = new SelectList(_context.Address, "Id", "AddressLine1");
            ViewData["UserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id");
            return View();
        }

        // POST: Reservations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,ReservationDate,Status,TotalAmount,IsDeleted,AddressId")] Reservation reservation)
        {
            
                _context.Add(reservation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            
            ViewData["AddressId"] = new SelectList(_context.Address, "Id", "AddressLine1", reservation.AddressId);
            ViewData["UserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", reservation.UserId);
            return View(reservation);
        }

        // GET: Reservations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Reservations == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null)
            {
                return NotFound();
            }
            ViewData["AddressId"] = new SelectList(_context.Address, "Id", "AddressLine1", reservation.AddressId);
            ViewData["UserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", reservation.UserId);
            return View(reservation);
        }

        // POST: Reservations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,ReservationDate,Status,TotalAmount,IsDeleted,AddressId")] Reservation reservation)
        {
            if (id != reservation.Id)
            {
                return NotFound();
            }

           
                try
                {
                    _context.Update(reservation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservationExists(reservation.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            
            ViewData["AddressId"] = new SelectList(_context.Address, "Id", "AddressLine1", reservation.AddressId);
            ViewData["UserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", reservation.UserId);
            return View(reservation);
        }

        // GET: Reservations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Reservations == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations
                .Include(r => r.Address)
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // POST: Reservations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Reservations == null)
            {
                return Problem("Entity set 'MyContext.Reservations'  is null.");
            }
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation != null)
            {
                _context.Reservations.Remove(reservation);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReservationExists(int id)
        {
          return (_context.Reservations?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        public async Task<IActionResult> SearchByReservationrId(int? id)
        {
            var reservation = await _context.Reservations
                                            .Where(e => e.Id == id)
                                            .ToListAsync();

            return View("Index", reservation);
        }

    }
}
