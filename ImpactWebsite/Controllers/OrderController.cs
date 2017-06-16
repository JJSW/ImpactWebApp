using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ImpactWebsite.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using ImpactWebsite.Models;
using ImpactWebsite.Models.OrderModels;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using ImpactWebsite.Models.AccountViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ImpactWebsite.Controllers
{
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private IHostingEnvironment _environment;
        private readonly ILogger _logger;
        private static string _emailAddress;
        private static string _totalAmount;
        private static string _totalDay;
        private static int _orderId;
        private readonly string _externalCookieScheme;
        private int _dollarCent = 100;
        private static double _discountRate;

        public OrderController(ApplicationDbContext context,
                               UserManager<ApplicationUser> UserManager,
                               IHostingEnvironment environment,
                               SignInManager<ApplicationUser> SignInManager,
                               IOptions<IdentityCookieOptions> identityCookieOptions,
                               ILoggerFactory loggerFactory)
        {
            _context = context;
            _userManager = UserManager;
            _environment = environment;
            _signInManager = SignInManager;
            _logger = loggerFactory.CreateLogger<AccountController>();
            _externalCookieScheme = identityCookieOptions.Value.ExternalCookieAuthenticationScheme;
        }
        public async Task<IActionResult> Index(string message)
        {
            ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
            ViewData["error"] = message;
            _discountRate = 0;
            if (_signInManager.IsSignedIn(User))
            {
                _emailAddress = await _userManager.GetEmailAsync(user);
                ViewData["Email"] = _emailAddress;
            }
            List<TempOrder> tempOrders = new List<TempOrder>();

            var moduleList = _context.Modules.Include(o => o.UnitPrice);

            var activeDiscount1 = _context.Discounts.Where(w => w.IsActive == true).Where(x => x.DiscountName == "Discount1");
            var activeDiscount2 = _context.Discounts.Where(w => w.IsActive == true).Where(x => x.DiscountName == "Discount2");

            if (activeDiscount1.Any() || activeDiscount1 != null)
            {
                foreach (var active1 in activeDiscount1)
                {
                    ViewBag.D1SelectFrom = active1.SelectFrom;
                    ViewBag.D1SelectTo = active1.SelectTo;
                    ViewBag.D1DiscountRate = active1.DiscountRate;
                }
            }

            if (activeDiscount2.Any() || activeDiscount2 != null)
            {
                foreach (var active2 in activeDiscount2)
                {
                    ViewBag.D2SelectFrom = active2.SelectFrom;
                    ViewBag.D2SelectTo = active2.SelectTo;
                    ViewBag.D2DiscountRate = active2.DiscountRate;
                }
            }

            foreach (var module in moduleList)
            {
                tempOrders.Add(new TempOrder()
                {
                    Modules = module
                });
            }

            return View(tempOrders);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult NewOrder()
        {
            ViewData["DeliverDate"] = DateTime.Now.AddDays(Convert.ToDouble(_totalDay)).ToString("MMM dd yyyy");
            ViewData["TotalDay"] = _totalDay;
            ViewData["TotalAmount"] = _totalAmount;
            ViewData["LoggedinUserId"] = _context.Orders.FirstOrDefault(o => o.OrderId == _orderId).UserId;
            ViewData["orderId"] = _orderId;
            ViewData["DiscountRate"] = _discountRate;
            var OrderDetails = _context.OrderDetails.Where(o => o.OrderId == _orderId).Include(o => o.Module.UnitPrice);
            return View(OrderDetails.ToList());
        }

        [HttpPost]
        public async Task<IActionResult> NewOrder(IFormCollection collection, string email, string totalPrice, string totalDay)
        {
            int totalAmount = 0;
            int parsedAmount = 0;
            ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
            ApplicationUser TempUser;
            _totalAmount = totalPrice;
            _totalDay = totalDay;
            ViewData["DeliverDate"] = DateTime.Now.AddDays(Convert.ToDouble(_totalDay)).ToString("MMM dd yyyy");
            ViewData["TotalDay"] = totalDay;
            ViewData["TotalAmount"] = totalPrice;
            ViewData["DiscountRate"] = _discountRate;

            if (_signInManager.IsSignedIn(User))
            {
                TempUser = user;
                _emailAddress = await _userManager.GetEmailAsync(user);
                ViewData["Email"] = _emailAddress;
            }
            else
            {
                var findUser = await _userManager.FindByEmailAsync(email);
                var notRegisteredUser = await _userManager.FindByEmailAsync("temp@user.com");
                if (findUser != null)
                {
                    TempUser = findUser;
                }
                else
                {
                    TempUser = notRegisteredUser;
                }
                _emailAddress = email;
            }
            ViewData["Email"] = email;
            ViewData["LoggedinUserId"] = TempUser.Id;

            totalAmount = int.TryParse(_totalAmount, out parsedAmount) ? parsedAmount : 0;


            _context.Orders.Add(new Order()
            {
                UserEmail = email,
                OrderedDate = DateTime.Now,
                DeliveredDate = DateTime.Now.AddDays(Convert.ToDouble(_totalDay)),
                UserId = TempUser.Id,
                TotalAmount = totalAmount * _dollarCent
            });

            await _context.SaveChangesAsync();

            _orderId = _context.Orders.LastOrDefault(o => o.UserId == TempUser.Id).OrderId;

            CreateOrderDetail(collection);

            try
            {
                var newOrder = _context.Orders.SingleOrDefault(x => x.OrderId == _orderId);
                ViewData["OrderId"] = newOrder.OrderId;

                if (ViewData["OrderId"] == null)
                {
                    throw new ArgumentNullException();
                }
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException source: {0}", e.Source);
            }

            var OrderDetails = _context.OrderDetails.Where(o => o.OrderId == _orderId).Include(o => o.Module.UnitPrice);

            ViewData["orderId"] = _orderId;




            return View(OrderDetails.ToList());
        }

        private void CreateOrderDetail(IFormCollection collection)
        {
            var lists = collection["modules"];
            foreach (var list in lists)
            {
                var jsonObj = JsonConvert.DeserializeObject<TempOrder>(list);

                _context.OrderDetails.Add(new OrderDetail()
                {
                    ModifiedDate = DateTime.Now,
                    OrderId = _context.Orders.LastOrDefault(o => o.OrderId == _orderId).OrderId,
                    ModuleId = jsonObj.Modules.ModuleId,
                    ModuleName = jsonObj.Modules.ModuleName
                });
            }
            _context.SaveChanges();
        }

        private void GetOrderId()
        {

        }


        // GET: Orders
        public async Task<IActionResult> Orders()
        {
            var applicationDbContext = _context.Orders;
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Orders/OrderDetails/5
        public async Task<IActionResult> OrderDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderDetail = await _context.OrderDetails
                .Include(o => o.Module)
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (orderDetail == null)
            {
                return NotFound();
            }

            return View(orderDetail);
        }



        [HttpGet]
        public IActionResult SubmitPromoCode()
        {
            Promotion promotion = new Promotion();
            return PartialView(promotion);
        }

        [HttpPost]
        public async Task<IActionResult> SubmitPromoCode(Promotion model)
        {
            if (ModelState.IsValid)
            {
                var result = _context.Promotions.FirstOrDefault(p => p.PromotionCode.Equals(model.PromotionCode));
                if (result != null && result.DateFrom <= DateTime.Now && result.DateTo >= DateTime.Now && result.IsActive)
                {
                    _discountRate = result.DiscountRate;
                    var tmpAmount = _context.Orders.FirstOrDefault(o => o.OrderId == _orderId).TotalAmount;
                    //var discountRate = result.DiscountRate;
                    tmpAmount = tmpAmount - (tmpAmount * (int)(_discountRate / 100));
                    _context.Orders.FirstOrDefault(o => o.OrderId == _orderId).TotalAmount = tmpAmount;
                    await _context.SaveChangesAsync();
                }
                else
                {
                    return Json(new { success = false });
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult FileUpdaload()
        {
            ViewData["Email"] = _emailAddress;
            ViewData["TotalAmount"] = _totalAmount;
            return PartialView("_Investment");
        }

        [HttpPost]
        public async Task<IActionResult> FileUpdaload(ICollection<IFormFile> files)
        {
            ViewData["TotalAmount"] = _totalAmount;
            DateTime dtNow = DateTime.Now;
            string UpdateDate = dtNow.ToString("ddMMyyyy");
            string uploads = Path.Combine(_environment.WebRootPath, "uploads/" + UpdateDate + "/" + _emailAddress);

            if (!Directory.Exists(uploads))
            {
                Directory.CreateDirectory(uploads);
            }

            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    using (var fileStream = new FileStream(Path.Combine(uploads, file.FileName), FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                }
            }
            var OrderDetails = _context.OrderDetails.Where(o => o.OrderId == _orderId).Include(o => o.Module.UnitPrice);
            ViewData["orderId"] = _orderId;
            return RedirectToAction("NewOrder");
        }

        [HttpGet]
        public IActionResult GoRegisterPartial()
        {
            RegisterViewModel model = new RegisterViewModel();

            return PartialView("_Register", model);
        }

        [HttpGet]
        public IActionResult PartialModuleDetail(string id)
        {
            var DetailModules = _context.Modules.FirstOrDefault(m => m.ModuleId == Convert.ToInt32(id));
            return PartialView("_PartialModuleDetail", DetailModules);
        }

        [HttpGet]
        public async Task<IActionResult> RegisterLogin(int id)
        {
            ViewData["orderId"] = _orderId;
            string userEmail = _context.Orders.FirstOrDefault(o => o.OrderId == id).UserEmail;

            if (await _userManager.FindByEmailAsync(userEmail) == null)
            {
                ViewData["checkUser"] = "NeedRegister";
            }
            else
            {
                ViewData["checkUser"] = "NeedLogin";
            }

            return View("RegisterLogin");
        }
        //
        // GET: /Account/_Register
        [HttpGet]
        [AllowAnonymous]
        public IActionResult PartialRegister(string returnUrl = null)
        {
            ViewData["Email"] = _emailAddress;
            ViewData["orderId"] = _orderId;
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PartialRegister(PartialRegisterViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            ViewData["Email"] = _emailAddress;
            ViewData["orderId"] = _orderId;
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email, NewsletterRequired = model.NewsletterRequired };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    _context.Orders.FirstOrDefault(o => o.OrderId == _orderId).UserId = user.Id;
                    await _context.SaveChangesAsync();
                    _logger.LogInformation(3, "User created a new account with password.");
                    await _userManager.AddToRoleAsync(user, "Member");
                    return RedirectToAction("NewOrder");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/Login
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> PartialLogin(string returnUrl = null)
        {
            ViewData["Email"] = _emailAddress;
            ViewData["orderId"] = _orderId;
            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.Authentication.SignOutAsync(_externalCookieScheme);

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PartialLogin(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            ViewData["Email"] = _emailAddress;
            ViewData["orderId"] = _orderId;
            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    _logger.LogInformation(1, "User logged in.");
                    return RedirectToAction("NewOrder");
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning(2, "User account locked out.");
                    return View("Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(model);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }
        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

        #endregion
    }
}