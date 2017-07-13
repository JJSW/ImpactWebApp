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
    public class ModuleController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ModuleController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Module
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Modules.Include(o => o.UnitPrice);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Module/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderModule = await _context.Modules
                .Include(o => o.UnitPrice)
                .SingleOrDefaultAsync(m => m.ModuleId == id);
            if (orderModule == null)
            {
                return NotFound();
            }

            return View(orderModule);
        }

        // GET: Module/Create
        public IActionResult Create()
        {
            ViewData["UnitPriceId"] = new SelectList(_context.UnitPrices, "UnitPriceId", "UnitPriceId");
            return View();
        }

        // POST: Module/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ModuleId,ModuleName,Description,UnitPriceId")] Module orderModule)
        {
            if (ModelState.IsValid)
            {
                _context.Add(orderModule);
                await _context.SaveChangesAsync();

                var tempModuleId = orderModule.ModuleId;

                _context.Modules.SingleOrDefault(m => m.ModuleId == tempModuleId).ModifiedDate = DateTime.Now;
                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            ViewData["UnitPriceId"] = new SelectList(_context.UnitPrices, "UnitPriceId", "UnitPriceId", orderModule.UnitPriceId);
            return View(orderModule);
        }

        // GET: Module/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderModule = await _context.Modules.SingleOrDefaultAsync(m => m.ModuleId == id);
            if (orderModule == null)
            {
                return NotFound();
            }
            ViewData["UnitPriceId"] = new SelectList(_context.UnitPrices, "UnitPriceId", "UnitPriceId", orderModule.UnitPriceId);
            return View(orderModule);
        }

        // POST: Module/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ModuleId,ModuleName,Description,UnitPriceId")] Module orderModule)
        {
            if (id != orderModule.ModuleId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {

                _context.Update(orderModule);
                _context.Modules.SingleOrDefault(o => o.ModuleId == id).ModifiedDate = DateTime.Now;
                await _context.SaveChangesAsync();

            }
            ViewData["UnitPriceId"] = new SelectList(_context.UnitPrices, "UnitPriceId", "UnitPriceId", orderModule.UnitPriceId);
            return RedirectToAction("Details", new { id = id });
        }

        // GET: Module/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderModule = await _context.Modules
                .Include(o => o.UnitPrice)
                .SingleOrDefaultAsync(m => m.ModuleId == id);
            if (orderModule == null)
            {
                return NotFound();
            }

            return View(orderModule);
        }

        // POST: Module/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var orderModule = await _context.Modules.SingleOrDefaultAsync(m => m.ModuleId == id);
            _context.Modules.Remove(orderModule);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool OrderModuleExists(int id)
        {
            return _context.Modules.Any(e => e.ModuleId == id);
        }
    }
}
