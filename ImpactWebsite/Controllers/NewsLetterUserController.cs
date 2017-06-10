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
    public class NewsletterUserController : Controller
    {
        private readonly ApplicationDbContext _context;

        public NewsletterUserController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: NewsletterUser
        public async Task<IActionResult> Index()
        {
            return View(await _context.NewsletterUsers.ToListAsync());
        }

        // GET: NewsletterUser/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var newsLetterUser = await _context.NewsletterUsers
                .SingleOrDefaultAsync(m => m.NewsletterUserId == id);
            if (newsLetterUser == null)
            {
                return NotFound();
            }

            return View(newsLetterUser);
        }

        // GET: NewsletterUser/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: NewsletterUser/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NewsletterUserId,Email,isSubscribed,ModifiedDate")] NewsletterUser newsLetterUser)
        {
            if (ModelState.IsValid)
            {
                _context.Add(newsLetterUser);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(newsLetterUser);
        }

        // GET: NewsletterUser/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var newsLetterUser = await _context.NewsletterUsers.SingleOrDefaultAsync(m => m.NewsletterUserId == id);
            if (newsLetterUser == null)
            {
                return NotFound();
            }
            return View(newsLetterUser);
        }

        // POST: NewsletterUser/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("NewsletterUserId,Email,isSubscribed,ModifiedDate")] NewsletterUser newsLetterUser)
        {
            if (id != newsLetterUser.NewsletterUserId)
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
                    if (!NewsletterUserExists(newsLetterUser.NewsletterUserId))
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

        // GET: NewsletterUser/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var newsLetterUser = await _context.NewsletterUsers
                .SingleOrDefaultAsync(m => m.NewsletterUserId == id);
            if (newsLetterUser == null)
            {
                return NotFound();
            }

            return View(newsLetterUser);
        }

        // POST: NewsletterUser/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var newsLetterUser = await _context.NewsletterUsers.SingleOrDefaultAsync(m => m.NewsletterUserId == id);
            _context.NewsletterUsers.Remove(newsLetterUser);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool NewsletterUserExists(long id)
        {
            return _context.NewsletterUsers.Any(e => e.NewsletterUserId == id);
        }
    }
}
