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
    public class OrderLinesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrderLinesController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: OrderLines
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.OrderLines.Include(o => o.Module).Include(o => o.OrderHeader);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: OrderLines/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderLine = await _context.OrderLines
                .Include(o => o.Module)
                .Include(o => o.OrderHeader)
                .SingleOrDefaultAsync(m => m.OrderLineId == id);
            if (orderLine == null)
            {
                return NotFound();
            }

            return View(orderLine);
        }

        // GET: OrderLines/Create
        public IActionResult Create()
        {
            ViewData["ModuleId"] = new SelectList(_context.Modules, "ModuleId", "ModuleId");
            ViewData["OrderHeaderId"] = new SelectList(_context.OrderHeaders, "OrderHeaderId", "UserEmail");
            return View();
        }

        // POST: OrderLines/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderLineId,OrderHeaderId,ModuleId,ModuleName,ModifiedDate")] OrderLine orderLine)
        {
            if (ModelState.IsValid)
            {
                _context.Add(orderLine);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["ModuleId"] = new SelectList(_context.Modules, "ModuleId", "ModuleId", orderLine.ModuleId);
            ViewData["OrderHeaderId"] = new SelectList(_context.OrderHeaders, "OrderHeaderId", "UserEmail", orderLine.OrderHeaderId);
            return View(orderLine);
        }

        // GET: OrderLines/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderLine = await _context.OrderLines.SingleOrDefaultAsync(m => m.OrderLineId == id);
            if (orderLine == null)
            {
                return NotFound();
            }
            ViewData["ModuleId"] = new SelectList(_context.Modules, "ModuleId", "ModuleId", orderLine.ModuleId);
            ViewData["OrderHeaderId"] = new SelectList(_context.OrderHeaders, "OrderHeaderId", "UserEmail", orderLine.OrderHeaderId);
            return View(orderLine);
        }

        // POST: OrderLines/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderLineId,OrderHeaderId,ModuleId,ModuleName,ModifiedDate")] OrderLine orderLine)
        {
            if (id != orderLine.OrderLineId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(orderLine);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderLineExists(orderLine.OrderLineId))
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
            ViewData["ModuleId"] = new SelectList(_context.Modules, "ModuleId", "ModuleId", orderLine.ModuleId);
            ViewData["OrderHeaderId"] = new SelectList(_context.OrderHeaders, "OrderHeaderId", "UserEmail", orderLine.OrderHeaderId);
            return View(orderLine);
        }

        // GET: OrderLines/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderLine = await _context.OrderLines
                .Include(o => o.Module)
                .Include(o => o.OrderHeader)
                .SingleOrDefaultAsync(m => m.OrderLineId == id);
            if (orderLine == null)
            {
                return NotFound();
            }

            return View(orderLine);
        }

        // POST: OrderLines/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var orderLine = await _context.OrderLines.SingleOrDefaultAsync(m => m.OrderLineId == id);
            _context.OrderLines.Remove(orderLine);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool OrderLineExists(int id)
        {
            return _context.OrderLines.Any(e => e.OrderLineId == id);
        }
    }
}
