using bauen.DAL;
using bauen.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bauen.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly AppDbContext db;
        private readonly UserManager<AppUser> userManager;

        public UserController(AppDbContext _db, UserManager<AppUser> _userManager)
        {
            db = _db;
            userManager = _userManager;
        }

        public async Task<IActionResult> Index()
        {
            List<AppUser> users = await userManager.Users.ToListAsync();
            List<UserDTO> dtos = new List<UserDTO>();
            foreach (AppUser item in users)
            {
                UserDTO dto = new UserDTO()
                {
                    Id = item.Id,
                    FullName = item.FullName,
                    Email = item.Email,
                    UserName = item.UserName,
                    ProfilePhoto = item.ProfilePhoto,
                    IsActive = item.IsActive,
                    Role = (await userManager.GetRolesAsync(item))[0]
                };
                dtos.Add(dto);
            }
            return View(dtos);
        }
        public async Task<IActionResult> ChangeStatus(string id)
        {
            if (string.IsNullOrEmpty(id)) return RedirectToAction("Index", "User");
            AppUser user = await userManager.FindByIdAsync(id);
            UserDTO dto = new UserDTO
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                UserName = user.UserName,
                ProfilePhoto = user.ProfilePhoto,
                IsActive = user.IsActive,
                Role = (await userManager.GetRolesAsync(user))[0]
            };

            return View(dto);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeStatusConfirmed(string id)
        {
            if (string.IsNullOrEmpty(id)) return RedirectToAction("Index", "User");
            AppUser user = await userManager.FindByIdAsync(id);
            if (user.IsActive)
            {
                user.IsActive = false;
            }
            else
            {
                user.IsActive = true;
            }
            await db.SaveChangesAsync();
            return RedirectToAction("Index", "User");
        }
        public async Task<IActionResult> ChangeRole(string id)
        {
            if (string.IsNullOrEmpty(id)) return RedirectToAction("Index", "User");

            AppUser user = await userManager.FindByIdAsync(id);
            UserDTO dto = new UserDTO
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                UserName = user.UserName,
                ProfilePhoto = user.ProfilePhoto,
                IsActive = user.IsActive,
                Role = (await userManager.GetRolesAsync(user))[0],
                Roles = new List<string>() { "Member", "Admin" }
            };

            return View(dto);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeRoleConfirmed(string id, string role)
        {
            if (string.IsNullOrEmpty(id)) return RedirectToAction("Index", "User");
            if (string.IsNullOrEmpty(role)) return RedirectToAction("Index", "User");
            AppUser user = await userManager.FindByIdAsync(id);
            string CurrentRole = (await userManager.GetRolesAsync(user))[0];
            await userManager.RemoveFromRoleAsync(user, CurrentRole);
            await userManager.AddToRoleAsync(user, role);
            return RedirectToAction("Index", "User");
        }
        public async Task<IActionResult> ChangePassword(string id)
        {
            if (string.IsNullOrEmpty(id)) return RedirectToAction("Index", "User");

            AppUser user = await userManager.FindByIdAsync(id);
            UserDTO dto = new UserDTO
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                UserName = user.UserName,
                ProfilePhoto = user.ProfilePhoto,
                IsActive = user.IsActive,
                Role = (await userManager.GetRolesAsync(user))[0],
                Roles = new List<string>() { "Member", "Admin" }
            };

            return View(dto);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePasswordConfirmed(string id, string password, string repeatPassword)
        {
            if (string.IsNullOrEmpty(id)) return RedirectToAction("Index", "User");
            if (string.IsNullOrEmpty(password)) return RedirectToAction("Index", "User");
            if (string.IsNullOrEmpty(repeatPassword)) return RedirectToAction("Index", "User");
            if (password != repeatPassword) return RedirectToAction("Index", "User");

            AppUser user = await userManager.FindByIdAsync(id);
            string token = await userManager.GeneratePasswordResetTokenAsync(user);
            await userManager.ResetPasswordAsync(user, token, password);
            return RedirectToAction("Index", "User");
        }
    }
}
