using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using generalStore.Data;
using generalStore.Models;
using AspNetCoreHero.ToastNotification.Abstractions;
using generalStore.Helpper;
using generalStore.Extension;
using generalStore.Areas.Admin.Models;

namespace generalStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminAccountsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly INotyfService _toastNotification;

        public AdminAccountsController(ApplicationDbContext context, INotyfService toastNotification)
        {
            _context = context;
            _toastNotification = toastNotification;
        }

        // GET: Admin/AdminAccounts
        public async Task<IActionResult> Index()
        {
            ViewData["QuyenTruyCap"] = new SelectList(_context.Roles, "RoleId", "RoleDescription");

            List<SelectListItem> lsTrangThai = new List<SelectListItem>();
            lsTrangThai.Add(new SelectListItem() { Text = "Active", Value = "1" });
            lsTrangThai.Add(new SelectListItem() { Text = "Block", Value = "0" });
            ViewData["lsTrangThai"] = lsTrangThai;

            var applicationDbContext = _context.Accounts.Include(a => a.Role);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Admin/AdminAccounts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Accounts == null)
            {
                return NotFound();
            }

            var account = await _context.Accounts
                .Include(a => a.Role)
                .FirstOrDefaultAsync(m => m.AccountId == id);
            if (account == null)
            {
                return NotFound();
            }

            return View(account);
        }

        // GET: Admin/AdminAccounts/Create
        public IActionResult Create()
        {
            ViewData["QuyenTruyCap"] = new SelectList(_context.Roles, "RoleId", "RoleName");
            return View();
        }

        // POST: Admin/AdminAccounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AccountId,Phone,Email,Password,Salt,Active,FullName,RoleId,LastLogin,CreateDate")] Account account)
        {
            if (ModelState.IsValid)
            {
                string salt = Utilities.GetRandomKey();
                account.Salt = salt;

                account.Password = (account.Phone + salt.Trim()).ToMD5();
                account.CreateDate = DateTime.Now;


                _context.Add(account);
                await _context.SaveChangesAsync();
                _toastNotification.Success("Tạo mới tài khoản quản trị thành công");
                return RedirectToAction(nameof(Index));
            }
            ViewData["QuyenTruyCap"] = new SelectList(_context.Roles, "RoleId", "RoleName", account.RoleId);
            return View(account);
        }

        // GET: Admin/AdminAccounts/ChangePassword
        public IActionResult ChangePassword()
        {
            ViewData["QuyenTruyCap"] = new SelectList(_context.Roles, "RoleId", "RoleName");
            return View();
        }

        // POST: Admin/AdminAccounts/ChangePassword
        [HttpPost]
        public IActionResult ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var taikhoan = _context.Accounts.AsNoTracking().SingleOrDefault(x => x.Email == model.Email);
                if (taikhoan == null) return RedirectToAction("Login", "Accounts");

                var pass = (model.PasswordNow.Trim() + taikhoan.Salt.Trim()).ToMD5();
                if (pass == taikhoan.Password)
                {
                    String passnew = (model.Password.Trim() + taikhoan.Salt.Trim()).ToMD5();
                    taikhoan.Password = passnew;
                    taikhoan.LastLogin = DateTime.Now;
                    _context.Update(taikhoan);
                    _context.SaveChanges();
                    _toastNotification.Success("Change password success");
                    return RedirectToAction("Login", "Accounts", new { Area = "Admin" });
                }
                _toastNotification.Success("Wrong Change password");
                return RedirectToAction("Login", "Accounts");
            }
            return View();
        }
   

        // GET: Admin/AdminAccounts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Accounts == null)
            {
                return NotFound();
            }

            var account = await _context.Accounts.FindAsync(id);
            if (account == null)
            {
                return NotFound();
            }
            ViewData["QuyenTruyCap"] = new SelectList(_context.Roles, "RoleId", "RoleName", account.RoleId);
            return View(account);
        }

        // POST: Admin/AdminAccounts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AccountId,Phone,Email,Password,Salt,Active,FullName,RoleId,LastLogin,CreateDate")] Account account)
        {
            if (id != account.AccountId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(account);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AccountExists(account.AccountId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["QuyenTruyCap"] = new SelectList(_context.Roles, "RoleId", "RoleName", account.RoleId);
            return View(account);
        }

        // GET: Admin/AdminAccounts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Accounts == null)
            {
                return NotFound();
            }

            var account = await _context.Accounts
                .Include(a => a.Role)
                .FirstOrDefaultAsync(m => m.AccountId == id);
            if (account == null)
            {
                return NotFound();
            }

            return View(account);
        }

        // POST: Admin/AdminAccounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Accounts == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Accounts'  is null.");
            }
            var account = await _context.Accounts.FindAsync(id);
            if (account != null)
            {
                _context.Accounts.Remove(account);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AccountExists(int id)
        {
          return (_context.Accounts?.Any(e => e.AccountId == id)).GetValueOrDefault();
        }
    }
}
