using AspNetCoreHero.ToastNotification.Abstractions;
using generalStore.Data;
using generalStore.Extension;
using generalStore.Helpper;
using generalStore.Models;
using generalStore.Models.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace generalStore.Controllers
{
    [Authorize] 
    public class AccountsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly INotyfService _toastNotification;

        public AccountsController(ApplicationDbContext context, INotyfService toastNotification)
        {
            _context = context;
            _toastNotification = toastNotification;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ValidatePhone(string Phone)
        {
            try
            {
                var khachhang = _context.Customers.AsNoTracking().SingleOrDefault(x => x.Phone.ToLower() == Phone.ToLower());
                if (khachhang != null)
                    return Json(data: "Số điện thoại: " + Phone + " Đã được sử dụng ");
                return Json(data: true);
            }
            catch
            {
                return Json(data: true);
            }
        }

        public IActionResult ValidateEmail(string Email)
        {
            try
            {
                var khachhang = _context.Customers.AsNoTracking().SingleOrDefault(x => x.Phone.ToLower() == Email.ToLower());
                if (khachhang != null)
                    return Json(data: "Email: " + Email + " Đã được sử dụng ");
                return Json(data: true);
            }
            catch
            {
                return Json(data: true);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("tai-khoan-cua-toi", Name = "Dashboard")]
        public IActionResult Dashboard()
        {
            var taikhoanID = HttpContext.Session.GetString("CustomerId");
            if(taikhoanID != null)
            {
                var khachhang = _context.Customers.AsNoTracking().SingleOrDefault(x => x.CustomerId == Convert.ToInt32(taikhoanID));
                    if(khachhang != null)
                {
                    return View(khachhang);
                }
            }
            return RedirectToAction("Login", "Accounts");
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("dang-ky", Name = "Dangky")]
        public IActionResult DangKyTaiKhoan()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("dang-ky", Name = "Dangky")]
        public async Task<IActionResult> DangKyTaiKhoan(RegisterViewModel taikhoan)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    string salt = Utilities.GetRandomKey();
                    Customer khachhang = new Customer
                    {
                        FullName = taikhoan.FullName,
                        Phone = taikhoan.Phone.Trim().ToLower(),
                        Email = taikhoan.Email.Trim().ToLower(),
                        Password = (taikhoan.Password + salt.Trim()).ToMD5(),
                        Active = true,
                        Salt = salt,
                        CreateDate = DateTime.Now
                    };
                    try
                    {
                        _context.Add(khachhang);
                        await _context.SaveChangesAsync();
                        //luu session MaKh
                        HttpContext.Session.SetString("CustomerId", khachhang.CustomerId.ToString());
                        var taikhoanID = HttpContext.Session.GetString("CustomerId");
                        //Identity
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, khachhang.FullName),
                            new Claim("CustomerId", khachhang.CustomerId.ToString())
                        };
                        ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "login");
                        ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                        await HttpContext.SignInAsync(claimsPrincipal);
                        _toastNotification.Success("Register success!");
                        /*return RedirectToAction("Dashboard", "Accounts");*/
                        return RedirectToAction("Index", "Home");
                    }
                    catch(Exception ex)
                    {
                        return RedirectToAction("DangKyTaiKhoan", "Accounts");
                    }
                }
                else
                {
                    return View(taikhoan);
                }
            }
            catch
            {
                return View(taikhoan);
            }
        }
        [AllowAnonymous]
        [Route("dang-nhap", Name = "DangNhap")]
        public IActionResult Login(string? returnUrl = null)
        {
            /*var taikhoanID = HttpContext.Session.GetString("CustomerId");
            if(taikhoanID != null)
            {
                return RedirectToAction("Dashboard", "Account");
            }*/
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("dang-nhap", Name = "DangNhap")]
        public async Task<IActionResult> Login(LoginViewModel customer, string? returnUrl = null)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    bool isEmail = Utilities.IsValidEmail(customer.UserName);
                    if (!isEmail) return View(customer);

                    var khachhang = _context.Customers.AsNoTracking().SingleOrDefault(x => x.Email.Trim() == customer.UserName);

                    if (khachhang == null) return RedirectToAction("DangKyTaiKhoan");
                    string pass = (customer.Password + khachhang.Salt.Trim()).ToMD5();
                    if(khachhang.Password != pass)
                    {
                        _toastNotification.Success("Wrong password!");
                        return View(customer);
                    }

                    //Kiem tra xem account co bi disable hay ko

                    if(khachhang.Active == false)
                    {
                        return RedirectToAction("ThongBao", "Accounts");
                    }

                    //luu  Session MaKh
                    HttpContext.Session.SetString("CustomerId", khachhang.CustomerId.ToString());
                    var taikhoanID = HttpContext.Session.GetString("CustomerId");
                    //Identity
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, khachhang.FullName),
                        new Claim("CustomerId", khachhang.CustomerId.ToString())
                    };
                    ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "login");
                    ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                    /*var authProperties = new AuthenticationProperties
                    {
                        AllowRefresh = true,
                        ExpiresUtc = DateTimeOffset.Now.AddDays(1),
                        IsPersistent = true,
                    };*/
                    await HttpContext.SignInAsync(claimsPrincipal);
                    _toastNotification.Success("Login success!");
                    return RedirectToAction("Dashboard", "Accounts");
                }
            }
            catch(Exception ex)
            {
                return RedirectToAction("DangKyTaiKhoan", "Accounts");
            }

            return View(customer);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("logout", Name = "Logout")]
        public async Task<IActionResult> Logout(string? returnUrl = null)
        {
            HttpContext.Session.Remove("CustomerId");
            HttpContext.SignOutAsync();
            _toastNotification.Success("Logout success");
            return RedirectToAction("Index", "Home");
        }
    }
}
