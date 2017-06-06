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
    public class OrderHeadersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrderHeadersController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: OrderHeaders
        public async Task<IActionResult> Index()
        {
            return View(await _context.OrderHeaders.ToListAsync());
        }

        // GET: OrderHeaders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderHeader = await _context.OrderHeaders
                .SingleOrDefaultAsync(m => m.OrderHeaderId == id);
            if (orderHeader == null)
            {
                return NotFound();
            }

            return View(orderHeader);
        }

        // GET: OrderHeaders/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: OrderHeaders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderHeaderId,OrderNum,SalesRep,OrderedDate,DeliveredDate,OrderStatus,NoteFromUser,NoteFromAdmin,UserEmail,UserId,TotalAmount")] OrderHeader orderHeader)
        {
            if (ModelState.IsValid)
            {
                _context.Add(orderHeader);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(orderHeader);
        }

        // GET: OrderHeaders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderHeader = await _context.OrderHeaders.SingleOrDefaultAsync(m => m.OrderHeaderId == id);
            if (orderHeader == null)
            {
                return NotFound();
            }
            return View(orderHeader);
        }

        // POST: OrderHeaders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderHeaderId,OrderNum,SalesRep,OrderedDate,DeliveredDate,OrderStatus,NoteFromUser,NoteFromAdmin,UserEmail,UserId,TotalAmount")] OrderHeader orderHeader)
        {
            if (id != orderHeader.OrderHeaderId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(orderHeader);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderHeaderExists(orderHeader.OrderHeaderId))
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
            return View(orderHeader);
        }

        // GET: OrderHeaders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderHeader = await _context.OrderHeaders
                .SingleOrDefaultAsync(m => m.OrderHeaderId == id);
            if (orderHeader == null)
            {
                return NotFound();
            }

            return View(orderHeader);
        }

        // POST: OrderHeaders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var orderHeader = await _context.OrderHeaders.SingleOrDefaultAsync(m => m.OrderHeaderId == id);
            _context.OrderHeaders.Remove(orderHeader);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool OrderHeaderExists(int id)
        {
            return _context.OrderHeaders.Any(e => e.OrderHeaderId == id);
        }
    }
}
