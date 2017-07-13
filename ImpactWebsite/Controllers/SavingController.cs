using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ImpactWebsite.Data;
using ImpactWebsite.Models.OrderModels;

namespace ImpactWebsite.Controllers
{
    public class SavingController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SavingController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: Saving
        public async Task<IActionResult> Index()
        {
            return View(await _context.Savings.ToListAsync());
        }

        // GET: Saving/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var saving = await _context.Savings
                .SingleOrDefaultAsync(m => m.SavingId == id);
            if (saving == null)
            {
                return NotFound();
            }

            return View(saving);
        }

        // GET: Saving/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Saving/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SavingId,SavingName,SavingRate,SelectFrom,SelectTo,Description,IsActive")] Saving saving)
        {
            if (ModelState.IsValid)
            {
                _context.Add(saving);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(saving);
        }

        // GET: Saving/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var saving = await _context.Savings.SingleOrDefaultAsync(m => m.SavingId == id);
            if (saving == null)
            {
                return NotFound();
            }
            return View(saving);
        }

        // POST: Saving/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SavingId,SavingName,SavingRate,SelectFrom,SelectTo,Description,IsActive")] Saving saving)
        {
            if (id != saving.SavingId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(saving);
                    _context.Savings.SingleOrDefault(p => p.SavingId == id).ModifiedDate = DateTime.Now;
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SavingExists(saving.SavingId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(saving);
        }

        // GET: Saving/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var saving = await _context.Savings
                .SingleOrDefaultAsync(m => m.SavingId == id);
            if (saving == null)
            {
                return NotFound();
            }

            return View(saving);
        }

        // POST: Saving/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var saving = await _context.Savings.SingleOrDefaultAsync(m => m.SavingId == id);
            _context.Savings.Remove(saving);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool SavingExists(int id)
        {
            return _context.Savings.Any(e => e.SavingId == id);
        }
    }
}
