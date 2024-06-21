using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SONTM.WEB.Database;
using SONTM.WEB.Entities;
using SONTM.WEB.Helpers;
using System.Security.Claims;

namespace SONTM.WEB.Controllers
{
    [Route("admin/users")]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }
        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var query = from users in _context.Users

                        select new ApplicationUser
                        {
                            Id = users.Id,
                            IsBlock = users.IsBlock,
                            UserName = users.UserName,
                            CreatedById = users.CreatedById,
                            CreatedByName = users.CreatedByName,
                            CreatedDate = users.CreatedDate,
                            UpdatedById = users.UpdatedById,
                            UpdatedByName = users.UpdatedByName,
                            UpdatedDate = users.UpdatedDate,
                            Roles = (
                                        from userRoles in _context.UserRoles
                                        join roles in _context.Roles on userRoles.RoleId equals roles.Id
                                        where userRoles.UserId == users.Id
                                        select roles
                                    ).ToList()
                        };
            var userList = await query.ToListAsync();
            return View(userList);
        }
        [HttpGet("add")]
        public async Task<IActionResult> CreateUser()
        {
            ViewBag.RoleList = await _context.Roles.ToListAsync();
            return View();
        }
        [Authorize(Roles = "admin")]
        [HttpPost("add")]
        public async Task<IActionResult> CreateUser(ApplicationUser user)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var userName = User.Identity!.Name;
                await _context.Users.AddAsync(new ApplicationUser
                {
                    Id = Guid.NewGuid().ToString("N"),
                    CreatedById = userId,
                    CreatedByName = userName,
                    CreatedDate = DateTime.Now,
                    IsBlock = false,
                    UserName = user.UserName.Trim().ToLower(),
                    PasswordHash = PasswordHelper.HashPassword(user.Password),
                });
                foreach(var role in user.Roles)
                {
                    if(role.IsSelected)
                    {
                        await _context.UserRoles.AddAsync(new ApplicationUserRole
                        {
                            Id = Guid.NewGuid().ToString("N"),
                            CreatedById=userId,
                            CreatedByName=userName,
                            CreatedDate=DateTime.Now,
                            RoleId = role.Id,
                            RoleName = role.Name,
                            UserId = user.Id,
                            UserName = user.UserName
                        });
                    }
                }
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index", "User");
        }
    }
}
