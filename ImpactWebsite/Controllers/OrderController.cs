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
using System.Net.Http.Headers;

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
        private static string _selectionDiscount;
        private static string _totalAmountToPay;
        private static string _totalDay;
        private static string _promotionDiscountRate;
        private static PromotionStatusList _promotionStatus;
        private static int _promotionId;
        private static Int32 _orderId;
        private static string _orderNumber;
        private readonly string _externalCookieScheme;
        private int _dollarCent = 100; // $10.00 = 1000

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
            _promotionDiscountRate = "0";

            if (_signInManager.IsSignedIn(User))
            {
                _emailAddress = await _userManager.GetEmailAsync(user);
                ViewData["Email"] = _emailAddress;
            }

            List<TempOrderViewModel> tempOrders = new List<TempOrderViewModel>();

            var moduleList = _context.Modules.Include(o => o.UnitPrice);

            var activeDiscount1 = _context.Savings.Where(w => w.IsActive == true).Where(x => x.SavingId == 1);
            var activeDiscount2 = _context.Savings.Where(w => w.IsActive == true).Where(x => x.SavingId == 2);

            if (activeDiscount1.Any() || activeDiscount1 != null)
            {
                foreach (var active1 in activeDiscount1)
                {
                    ViewBag.D1SelectFrom = active1.SelectFrom;
                    ViewBag.D1SelectTo = active1.SelectTo;
                    ViewBag.D1DiscountRate = active1.SavingRate;
                }
            }

            if (activeDiscount2.Any() || activeDiscount2 != null)
            {
                foreach (var active2 in activeDiscount2)
                {
                    ViewBag.D2SelectFrom = active2.SelectFrom;
                    ViewBag.D2SelectTo = active2.SelectTo;
                    ViewBag.D2DiscountRate = active2.SavingRate;
                }
            }

            foreach (var module in moduleList)
            {
                tempOrders.Add(new TempOrderViewModel()
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
            ViewBag.SelectionDiscount = _selectionDiscount;
            ViewBag.PromotionStatus = _promotionStatus;
            TempData["PromotionDiscountRate"] = _promotionDiscountRate;
            ViewData["TotalAmountToPay"] = _totalAmountToPay;
            ViewData["LoggedinOrTempUserId"] = _context.Orders.SingleOrDefault(o => o.OrderId == _orderId).UserId;
            ViewData["OrderId"] = _orderId;
            ViewData["OrderNumber"] = _orderNumber;
            ViewData["Email"] = _emailAddress;

            var OrderDetails = _context.OrderDetails.Where(o => o.OrderId == _orderId).Include(o => o.Module.UnitPrice);
            return View(OrderDetails.ToList());
        }

        [HttpPost]
        public async Task<IActionResult> NewOrder(IFormCollection collection,
                                                  string email,
                                                  string selectionDiscount,
                                                  string totalAmountToPay,
                                                  string totalDay)
        {
            int parsedSelectionDiscount = 0;
            int parsedTotalAmountToPay = 0;
            _promotionDiscountRate = "0";

            ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
            ApplicationUser TempUser;

            _selectionDiscount = selectionDiscount;
            _totalAmountToPay = totalAmountToPay;
            _totalDay = totalDay;

            ViewData["DeliverDate"] = DateTime.Now.AddDays(Convert.ToDouble(_totalDay)).ToString("MMM dd yyyy");
            ViewData["TotalDay"] = _totalDay;
            ViewBag.SelectionDiscount = _selectionDiscount;
            ViewBag.PromotionStatus = PromotionStatusList.Ready;
            TempData["PromotionDiscountRate"] = _promotionDiscountRate;

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
            ViewData["Email"] = _emailAddress;

            parsedTotalAmountToPay = ParseStringToInt(_totalAmountToPay);
            parsedSelectionDiscount = ParseStringToInt(_selectionDiscount);

            ViewData["TotalAmountToPay"] = _totalAmountToPay;

            var ordersFromCurrentUser = _context.Orders.Where(o => o.UserEmail == _emailAddress);

            if (ordersFromCurrentUser == null || !ordersFromCurrentUser.Any())
            {
                _orderNumber = CreateOrderPattern() + "001";
            }
            else
            {
                var currentOrderNumber = ordersFromCurrentUser.OrderByDescending(o => o.OrderNum).FirstOrDefault().OrderNum;
                var orderPattern = CreateOrderPattern();
                var orderSequence = Convert.ToInt32(currentOrderNumber.Substring(7));
                var nextOrderSequece = orderSequence + 1;
                _orderNumber = orderPattern + nextOrderSequece.ToString("D3");
            }

            _context.Orders.Add(new Order()
            {
                UserEmail = _emailAddress,
                OrderedDate = DateTime.Now,
                UserId = TempUser.Id,
                TotalAmount = parsedTotalAmountToPay * _dollarCent,
                SelectionDiscount = parsedSelectionDiscount,
                PromotionId = -1,
                OrderNum = _orderNumber,                
            });

            await _context.SaveChangesAsync();

            _orderId = _context.Orders.LastOrDefault(o => o.UserId == TempUser.Id).OrderId;

            CreateOrderDetail(collection);
            CreateModuleIds();

            var OrderDetails = _context.OrderDetails.Where(od => od.OrderId == _orderId).Include(o => o.Module.UnitPrice);

            ViewData["OrderId"] = _orderId;
            ViewData["OrderNumber"] = _orderNumber;
            return View(OrderDetails.ToList());
        }

        private int ParseStringToInt(string str)
        {
            int parsedInt = 0;
            int parsedResult = 0;

            if (int.TryParse(str, out parsedInt))
            {
                parsedResult = parsedInt;
            }
            else
            {
                _logger.LogWarning(1, "Fail to parse the string to int.");
                parsedResult = 0;
            }

            return parsedResult;
        }

        private void CreateOrderDetail(IFormCollection collection)
        {
            var lists = collection["modules"];
            foreach (var list in lists)
            {
                var jsonObj = JsonConvert.DeserializeObject<TempOrderViewModel>(list);

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

        private string CreateOrderPattern()
        {
            string today = DateTime.Today.ToString("MMdd");
            return "IM" + today;
        }

        private void CreateModuleIds()
        {
            var moduleIdList = _context.OrderDetails.Where(od => od.OrderId == _orderId).Select(s => s.ModuleId);
            var moduleIds = "";
            foreach (var moduleId in moduleIdList)
            {
                moduleIds += moduleId + " ";
            }

            _context.Orders.SingleOrDefault(o => o.OrderId == _orderId).ModuleIds = moduleIds;
            _context.SaveChanges();
        }

        // GET: Orders
        public async Task<IActionResult> Orders()
        {
            var applicationDbContext = _context.Orders;
            return View(await applicationDbContext.ToListAsync());
        }

        [HttpGet]
        public IActionResult SubmitPromoCode()
        {
            Promotion promotion = new Promotion();
            return PartialView(promotion);
        }

        [HttpPost]
        public async Task<IActionResult> SubmitPromoCode(Promotion promotion)
        {
            if (ModelState.IsValid)
            {
                var verfiedPromotion = _context.Promotions.FirstOrDefault(p => p.PromotionCode.Equals(promotion.PromotionCode));
                var verfiedPromotionId = verfiedPromotion.PromotionId;
                bool isPromotionCodeAppliedToOrder = _context.Orders.SingleOrDefault(s => s.OrderId == _orderId).IsPromotionCodeApplied;

                var userId = _userManager.GetUserId(HttpContext.User);
                var allOrdersFromCurrentUser = _context.Orders.Where(o => o.UserId == userId);
                bool isPromotionCodeAppliedToUser = allOrdersFromCurrentUser.Select(a => a.PromotionId).Any(a => a.Equals(verfiedPromotionId));

                if (verfiedPromotion != null
                    && verfiedPromotion.DateFrom <= DateTime.Now
                    && verfiedPromotion.DateTo >= DateTime.Now
                    && verfiedPromotion.IsActive)
                {
                    // Check if there is already a promotion code is applied
                    if (isPromotionCodeAppliedToOrder == false
                        && isPromotionCodeAppliedToUser == false)
                    {
                        var tempTotalAmount = _context.Orders.SingleOrDefault(o => o.OrderId == _orderId).TotalAmount;

                        if (verfiedPromotion.DiscountMethod == DiscountMethodList.Fixed)
                        {
                            var promotionDiscountRate = verfiedPromotion.DiscountRate;
                            tempTotalAmount = (tempTotalAmount - (promotionDiscountRate * _dollarCent));
                            _promotionDiscountRate = "-$" + promotionDiscountRate;
                            TempData["PromotionDiscountRate"] = _promotionDiscountRate;
                        }
                        else if (verfiedPromotion.DiscountMethod == DiscountMethodList.Percentage)
                        {
                            var tempTotalAmountWithCent = tempTotalAmount / _dollarCent;
                            var promotionDiscountRate = verfiedPromotion.DiscountRate;
                            var promotionDicountRatePercent = promotionDiscountRate * 0.01;
                            tempTotalAmount = (int)((tempTotalAmountWithCent - (tempTotalAmountWithCent * promotionDicountRatePercent)) * _dollarCent);
                            _promotionDiscountRate = "-" + promotionDiscountRate + "%";
                            TempData["PromotionDiscountRate"] = _promotionDiscountRate;
                        }

                        _context.Orders.SingleOrDefault(o => o.OrderId == _orderId).TotalAmount = tempTotalAmount;
                        _context.Orders.SingleOrDefault(o => o.OrderId == _orderId).PromotionId = verfiedPromotionId;
                        _promotionId = verfiedPromotionId;

                        await _context.SaveChangesAsync();

                        _totalAmountToPay = (tempTotalAmount / _dollarCent).ToString();

                        ViewBag.PromotionStatus = PromotionStatusList.Applied;
                        _promotionStatus = PromotionStatusList.Applied;
                    }
                    else
                    {
                        ViewBag.PromotionStatus = PromotionStatusList.Used;
                        _promotionStatus = PromotionStatusList.Used;
                    }
                }
                else
                {
                    return Json(new { success = false });
                }
            }

            return View(promotion);
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile file, string noteFromUser)
        {
            ViewData["TotalAmountToPay"] = _totalAmountToPay;
            string uploadDate = DateTime.Now.ToString("ddMMyyyy");
            string uploadPath = Path.Combine(_environment.WebRootPath, "uploads/" + uploadDate + "/" + _emailAddress);
            string uploadPathLink = HttpContext.Request.Scheme + "://" +
                                    HttpContext.Request.Host +
                                    "/uploads/" + uploadDate + "/" + _emailAddress + "/";

            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            if (file.Length > 0)
            {
                using (var fileStream = new FileStream(Path.Combine(uploadPath, file.FileName), FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                _context.Orders.SingleOrDefault(o => o.OrderId == _orderId).UploadedFileName += file.FileName + " ";
            }

            if (noteFromUser != null)
            {
                _context.Orders.SingleOrDefault(o => o.OrderId == _orderId).NoteFromUser = noteFromUser;
            }

            _context.Orders.SingleOrDefault(o => o.OrderId == _orderId).UploadedFilePath = uploadPathLink;

            ViewData["OrderId"] = _orderId;

            await _context.SaveChangesAsync();

            return RedirectToAction("NewOrder");
        }
      
        [HttpPost]
        public string SubmitNote(string noteFromUser)
        {
            if (noteFromUser != null)
            {
                _context.Orders.SingleOrDefault(o => o.OrderId == _orderId).NoteFromUser = noteFromUser;
            }

            _context.SaveChanges();

            return "Saved";
        }        

        [HttpGet]
        public IActionResult GoRegisterPartial()
        {
            RegisterViewModel model = new RegisterViewModel();

            return PartialView("_Register", model);
        }

        [HttpGet]
        public IActionResult ModuleDescription(string id)
        {
            var module = _context.Modules.FirstOrDefault(m => m.ModuleId == Convert.ToInt32(id));
            return PartialView("_ModuleDescription", module);
        }

        [HttpGet]
        public async Task<IActionResult> RegisterLogin(int id)
        {
            ViewData["OrderId"] = _orderId;
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
            ViewData["OrderId"] = _orderId;
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
            ViewData["OrderId"] = _orderId;
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
            ViewData["OrderId"] = _orderId;
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
            ViewData["OrderId"] = _orderId;
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