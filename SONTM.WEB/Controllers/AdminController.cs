using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SONTM.WEB.Database;
using SONTM.WEB.Entities;
using SONTM.WEB.Models;
using System.Security.Claims;
using SONTM.WEB.Helpers;

namespace SONTM.WEB.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == model.UserName);

                if (user != null && VerifyPassword(model.Password, user.PasswordHash))
                {
                    // Đăng nhập thành công, thiết lập cookie và chuyển hướng
                    var query = from userRoles in _context.UserRoles
                                join roles in _context.Roles on userRoles.RoleId equals roles.Id
                                where userRoles.UserId == user.Id
                                select roles.Name;
                    var allRoles = await query.ToListAsync();
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.Id),
                        new Claim(ClaimTypes.Name, user.UserName),
                    };
                    if(allRoles.Any())
                    {
                        foreach (var role in allRoles)
                        {
                            claims.Add(new Claim(ClaimTypes.Role, role));
                        }
                    }

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        private bool VerifyPassword(string enteredPassword, string storedHashedPassword)
        {
            // Logic để so sánh mật khẩu đã hash với mật khẩu người dùng nhập vào
            return PasswordHelper.IsValidPassword(enteredPassword, storedHashedPassword);
        }
    }
}
