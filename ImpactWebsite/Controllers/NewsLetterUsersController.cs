using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ImpactWebsite.Data;
using ImpactWebsite.Models;

namespace ImpactWebsite.Controllers
{
    public class NewsLetterUsersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public NewsLetterUsersController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: NewsLetterUsers
        public async Task<IActionResult> Index()
        {
            return View(await _context.NewsLetterUsers.ToListAsync());
        }

        // GET: NewsLetterUsers/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var newsLetterUser = await _context.NewsLetterUsers
                .SingleOrDefaultAsync(m => m.NewsLetterUserId == id);
            if (newsLetterUser == null)
            {
                return NotFound();
            }

            return View(newsLetterUser);
        }

        // GET: NewsLetterUsers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: NewsLetterUsers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NewsLetterUserId,Email,isSubscribed,ModifiedDate")] NewsLetterUser newsLetterUser)
        {
            if (ModelState.IsValid)
            {
                _context.Add(newsLetterUser);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(newsLetterUser);
        }

        // GET: NewsLetterUsers/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var newsLetterUser = await _context.NewsLetterUsers.SingleOrDefaultAsync(m => m.NewsLetterUserId == id);
            if (newsLetterUser == null)
            {
                return NotFound();
            }
            return View(newsLetterUser);
        }

        // POST: NewsLetterUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("NewsLetterUserId,Email,isSubscribed,ModifiedDate")] NewsLetterUser newsLetterUser)
        {
            if (id != newsLetterUser.NewsLetterUserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(newsLetterUser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NewsLetterUserExists(newsLetterUser.NewsLetterUserId))
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
            return View(newsLetterUser);
        }

        // GET: NewsLetterUsers/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var newsLetterUser = await _context.NewsLetterUsers
                .SingleOrDefaultAsync(m => m.NewsLetterUserId == id);
            if (newsLetterUser == null)
            {
                return NotFound();
            }

            return View(newsLetterUser);
        }

        // POST: NewsLetterUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var newsLetterUser = await _context.NewsLetterUsers.SingleOrDefaultAsync(m => m.NewsLetterUserId == id);
            _context.NewsLetterUsers.Remove(newsLetterUser);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool NewsLetterUserExists(long id)
        {
            return _context.NewsLetterUsers.Any(e => e.NewsLetterUserId == id);
        }
    }
}
