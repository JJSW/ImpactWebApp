using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ImpactWebsite.Models;
using ImpactWebsite.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ImpactWebsite.Controllers
{
    public class OrderSummaryController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public OrderSummaryController(ApplicationDbContext context,
                                      UserManager<ApplicationUser> UserManager)
        {
            _context = context;
            _userManager = UserManager;        
        }

        public async Task<IActionResult> Index()
        {
            ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
            var userId = user.Id;

            return View(await _context.Orders.Where(m => m.UserId == userId).ToListAsync());
        }


        public async Task<IActionResult> OrderDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderDetails = await _context.OrderDetails.Where(o => o.OrderId == id).ToListAsync();

            if (orderDetails == null)
            {
                return NotFound();
            }

            return View(orderDetails);
        }
    }
}