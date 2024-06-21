using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SONTM.WEB.Database;
using SONTM.WEB.Entities;
using SONTM.WEB.Helpers;
using System.Security.Claims;

namespace SONTM.WEB.Controllers
{
    [Authorize(Roles = "admin")]
    [Route("admin/roles")]
    public class RoleController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RoleController(ApplicationDbContext context)
        {
            _context = context;
        }
       
        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var query = _context.Roles;
            var roleList = await query.ToListAsync();
            return View(roleList);
        }
       
        [HttpGet("add")]
        public async Task<IActionResult> CreateRole()
        {
            return View();
        }
       
        [HttpPost("add")]
        public async Task<IActionResult> CreateRole(ApplicationRole role)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var userName = User.Identity!.Name;
                await _context.Roles.AddAsync(new ApplicationRole
                {
                    Id = Guid.NewGuid().ToString("N"),
                    CreatedById = userId,
                    CreatedByName = userName,
                    CreatedDate = DateTime.Now,
                    Code = role.Code,
                    Name = role.Name,
                    Description = role.Description,
                });
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("RoleList", "Admin");
        }
    }
}
