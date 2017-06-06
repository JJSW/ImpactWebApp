using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ImpactWebsite.Data;
using ImpactWebsite.Models;
using Microsoft.Extensions.Options;
using ImpactWebsite.Common;

namespace ImpactWebsite.Controllers
{
    
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            ViewData["status"] = "get";
            NewsLetterUser nlUser = new NewsLetterUser();
            return View(nlUser);
        }

        [HttpPost]        
        public async Task<IActionResult> Index(NewsLetterUser model)
        {
            if (ModelState.IsValid)
            {
                if (_context.NewsLetterUsers.Any(a => a.Email == model.Email))
                {
                    ModelState.AddModelError("Email", "Email " + model.Email + " is already in use.");
                    ViewData["status"] = "fail";
                    return View(model);
                }
                model.ModifiedDate = DateTime.Now;
                _context.Add(model);
                await _context.SaveChangesAsync();
                ViewData["status"] = "success";
                return View(model);
            }
            return View(model);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View("~/Views/Home/Contact.cshtml");
        }
        public IActionResult Error()
        {
            return View("~/Views/Shared/Error.cshtml");
        }
    }
}
