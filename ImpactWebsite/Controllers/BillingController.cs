using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using ImpactWebsite.Models.BillingModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Security.Principal;
using ImpactWebsite.Common;
using ImpactWebsite.Models;
using Microsoft.AspNetCore.Authorization;
using ImpactWebsite.Data;
using ImpactWebsite.Models.OrderModels;
using Microsoft.EntityFrameworkCore;

namespace ImpactWebsite.Controllers
{
    [Authorize]
    public class BillingController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;
        private static int _amountInt;
        private static string _emailAddress;
        // To be able to sent total amount to Stripe API, makes cent digits to 100
        private int _dollarCent = 100;

        public BillingController(ApplicationDbContext context,
                                 UserManager<ApplicationUser> userManager,
                                 SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        /// Return billing page with a data that contains information with order and module.
        /// Only displays the specific order of the logged in user.
        /// Billing address will be displayed in any circumstances.
        public async Task<IActionResult> Index(string id, int orderId)
        {
            ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
            List<BillingDetailViewModel> billingVM = new List<BillingDetailViewModel>();
            var totalAmount = 0;
            var moduleCount = 0;

            if (_signInManager.IsSignedIn(User))
            {
                _emailAddress = await _userManager.GetEmailAsync(user);
                ViewData["Email"] = _emailAddress;
            }

            if (id != null)
            {
                var billingDetails = (from u in _context.Users
                                      join o in _context.Orders on u.Id equals o.UserId
                                      join od in _context.OrderDetails on o.OrderId equals od.OrderId
                                      join m in _context.Modules on od.ModuleId equals m.ModuleId
                                      select new
                                      {
                                          OrderId = o.OrderId,
                                          OrderNum = o.OrderNum,
                                          OrderedDate = o.OrderedDate,
                                          UserId = u.Id,
                                          ModuleName = m.ModuleName,
                                          UnitPrice = m.UnitPrice.Price,
                                          TotalAmount = o.TotalAmount,
                                          OrderStatus = o.OrderStatus,
                                          NoteFromUser = o.NoteFromUser,
                                          UploadedFileName = o.UploadedFileName,
                                      }).ToList();

                var temps = billingDetails.Where(x => x.UserId == id).Where(y => y.OrderId == orderId).ToList();

                foreach (var billing in temps)
                {
                    billingVM.Add(new BillingDetailViewModel()
                    {
                        OrderId = billing.OrderId,
                        OrderNum = billing.OrderNum,
                        OrderedDate = billing.OrderedDate,
                        UserId = billing.UserId,
                        ModuleName = billing.ModuleName,
                        UnitPrice = billing.UnitPrice,
                        TotalAmount = billing.TotalAmount,
                        OrderStatus = billing.OrderStatus,
                        NoteFromUser = billing.NoteFromUser,
                        UploadedFileName = billing.UploadedFileName,
                    });
                };

                foreach (var billing in billingVM)
                {
                    moduleCount += 1;
                    totalAmount = billing.TotalAmount;
                }

                ViewBag.BillingDetails = billingVM;

                ViewData["Amount"] = totalAmount;
                ViewData["AmountDisplay"] = totalAmount / _dollarCent;
                ViewData["ModuleCount"] = moduleCount;
                ViewData["OrderId"] = orderId;
            }

            var userId = await _userManager.GetUserIdAsync(user);
            var billingAddress = await _context.BillingAddresses.LastOrDefaultAsync(x => x.UserId == userId);

            ViewData["BillingAddressId"] = (billingAddress != null) ? (int)billingAddress.BillingAddressId : -1;
            ViewBag.BillingAddress = billingAddress;
            _amountInt = totalAmount;

            return View(billingVM);
        }

        /// Get order when a user select the default module.
        /// Since the default module is free of charge, no need to go through payment process.
        public async Task<IActionResult> ChargeDefault(int orderId)
        {
            var completedOrders = _context.Orders.Where(x => x.OrderId == orderId);

            foreach (var order in completedOrders)
            {
                order.OrderStatus = OrderStatusList.Processing;
            }

            await _context.SaveChangesAsync();

            return View(completedOrders);
        }

        /// Execute Stripe API.  
        /// Create customer and charge tokens for request.
        /// After successful payment, the order's status set to completed.
        public async Task<IActionResult> Charge(string stripeToken,
                                                string stripeEmail,
                                                int orderId,
                                                int bAddressId)
        {
            var customers = new StripeCustomerService();
            var charges = new StripeChargeService();

            var completedOrders = _context.Orders.Where(x => x.OrderId == orderId);

            var customer = customers.Create(new StripeCustomerCreateOptions
            {
                Email = stripeEmail,
                SourceToken = stripeToken,
            });

            var billingAddress = await _context.BillingAddresses.LastOrDefaultAsync(x => x.BillingAddressId == bAddressId);

            var charge = charges.Create(new StripeChargeCreateOptions
            {
                Amount = _amountInt,
                Description = "Module Charge",
                Currency = "cad",
                CustomerId = customer.Id,
            });

            /*
            StripeAddress stripeAddress = new StripeAddress()
            {
                Line1 = billingAddress.AddressLine1,
                Line2 = billingAddress.AddressLine2,
                CityOrTown = billingAddress.City,
                State = billingAddress.State,
                PostalCode = billingAddress.ZipCode,
                Country = billingAddress.Country
            };
            */

            foreach (var order in completedOrders) { order.OrderStatus = OrderStatusList.Processing; }

            _context.Orders.SingleOrDefault(o => o.OrderId == orderId).IsPromotionCodeApplied = true;

            await _context.SaveChangesAsync();

            return View(completedOrders);
        }

        public IActionResult Error()
        {
            return View();
        }

        /// Displays billing address for the logged in user.
        public async Task<IActionResult> BillingAddress()
        {
            var userId = User.GetUserId();
            var user = await _userManager.FindByIdAsync(userId);
            var billingAddress = await _context.BillingAddresses.LastOrDefaultAsync(w => w.UserId == userId);

            return View(billingAddress);
        }

        /// Get billing address for the logged in user.
        /// A user is able to have only one billing address, 
        /// so any updates will overwrite the previous address.
        public async Task<IActionResult> BillingAddress(BillingAddress billingAddress)
        {
            if (ModelState.IsValid)
            {
                var userId = User.GetUserId();
                var user = await _userManager.FindByIdAsync(userId);
                var currentBillingAddresses = _context.BillingAddresses
                    .Where(x => x.UserId == userId).ToList();

                if (currentBillingAddresses.Any())
                {
                    foreach (var address in currentBillingAddresses)
                    {
                        _context.BillingAddresses.Remove(address);
                    }

                    await _context.SaveChangesAsync();
                }

                billingAddress.UserId = userId;
                user.BillingAddress = billingAddress;
                await _userManager.UpdateAsync(user);

                return RedirectToAction("Index");
            }

            return View(billingAddress);
        }

    }
}