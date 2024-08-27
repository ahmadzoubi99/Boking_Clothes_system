using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Booking_clothes.Data;
using Booking_clothes.Models;
using System.Security.Claims;

namespace Booking_clothes.Controllers
{
    public class ContactUsController : Controller
    {
        private readonly MyContext _context;

        public ContactUsController(MyContext context)
        {
            _context = context;
        }

        // GET: ContactUs
        public async Task<IActionResult> Index()
        {
            var myContext = _context.ContactUsMessages.Include(c => c.User);
            return View(await myContext.ToListAsync());
        }

        // GET: ContactUs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ContactUsMessages == null)
            {
                return NotFound();
            }

            var contactUs = await _context.ContactUsMessages
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contactUs == null)
            {
                return NotFound();
            }

            return View(contactUs);
        }

        // GET: ContactUs/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id");
            return View();
        }

        // POST: ContactUs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateContact(string Name, string Email,string Subject, string Message,DateTime CreatedAt)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var contactUs = new ContactUs
            {
                Name = Name,
                Email = Email,
                Subject = Subject,
                Message = Message,
                CreatedAt = CreatedAt,
                UserId = userId

            };
            _context.Add(contactUs);
            await _context.SaveChangesAsync();
            return RedirectToAction("ContactUs", "Home");

            ViewData["UserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", contactUs.UserId);
            return View(contactUs);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,Name,Email,Subject,Message,CreatedAt,IsDeleted")] ContactUs contactUs)
        {
         
                _context.Add(contactUs);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            
            ViewData["UserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", contactUs.UserId);
            return View(contactUs);
        }

        // GET: ContactUs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ContactUsMessages == null)
            {
                return NotFound();
            }

            var contactUs = await _context.ContactUsMessages.FindAsync(id);
            if (contactUs == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", contactUs.UserId);
            return View(contactUs);
        }

        // POST: ContactUs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,Name,Email,Subject,Message,CreatedAt,IsDeleted")] ContactUs contactUs)
        {
            if (id != contactUs.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(contactUs);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContactUsExists(contactUs.Id))
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
            ViewData["UserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", contactUs.UserId);
            return View(contactUs);
        }

        // GET: ContactUs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ContactUsMessages == null)
            {
                return NotFound();
            }

            var contactUs = await _context.ContactUsMessages
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contactUs == null)
            {
                return NotFound();
            }

            return View(contactUs);
        }

        // POST: ContactUs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ContactUsMessages == null)
            {
                return Problem("Entity set 'MyContext.ContactUsMessages'  is null.");
            }
            var contactUs = await _context.ContactUsMessages.FindAsync(id);
            if (contactUs != null)
            {
                _context.ContactUsMessages.Remove(contactUs);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ContactUsExists(int id)
        {
          return (_context.ContactUsMessages?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
