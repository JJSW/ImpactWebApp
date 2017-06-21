using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ImpactWebsite.Models;
using ImpactWebsite.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ImpactWebsite.Models.OrderModels;

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

        public async Task<IActionResult> OrderDetails(int orderId)
        {
            if (orderId == 0)
            {
                return View("~/Views/Shared/Error.cshtml");
            }

            ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
            List<OrderDetailViewModel> orderDetailVM = new List<OrderDetailViewModel>();
            var userId = user.Id;

            var orderDetails = (from u in _context.Users
                                  join o in _context.Orders on u.Id equals o.UserId
                                  join od in _context.OrderDetails on o.OrderId equals od.OrderId
                                  join m in _context.Modules on od.ModuleId equals m.ModuleId
                                  select new
                                  {
                                      OrderId = o.OrderId,
                                      OrderNum = o.OrderNum,
                                      UserId = u.Id,
                                      OrderDetailId = od.OrderDetailId,
                                      ModuleId = m.ModuleId,
                                      ModuleName = m.ModuleName,
                                      NoteFromUser = o.NoteFromUser,
                                      NoteFromAdmin = o.NoteFromAdmin,
                                      UploadedFileName = o.UploadedFileName,
                                      TotalAmount = o.TotalAmount,
                                  }).ToList();

            var currentOrderDetails = orderDetails.Where(x => x.UserId == userId)
                                                  .Where(y => y.OrderId == orderId)
                                                  .ToList();

            foreach (var orderDetail in currentOrderDetails)
            {
                orderDetailVM.Add(new OrderDetailViewModel()
                {
                    OrderId = orderDetail.OrderId,
                    OrderNum = orderDetail.OrderNum,
                    UserId = orderDetail.UserId,
                    OrderDetailId = orderDetail.OrderDetailId,
                    ModuleName = orderDetail.ModuleName,
                    NoteFromUser = orderDetail.NoteFromUser,
                    NoteFromAdmin = orderDetail.NoteFromAdmin,
                    UploadedFileName = orderDetail.UploadedFileName,
                    TotalAmount = orderDetail.TotalAmount,
                });
            };

            if (orderDetailVM == null)
            {
                return NotFound();
            }

            return View(orderDetailVM);
        }
    }
}