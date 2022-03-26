using bauen.Areas.Admin.Models;
using bauen.DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bauen.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles ="Admin")]
    public class DashBoardController : Controller
    {
        private readonly AppDbContext db;
        public DashBoardController(AppDbContext _db)
        {
            db = _db;
        }
        public async Task<IActionResult> Index()
        {
            AdminViewModel avm = new AdminViewModel()
            {
                BannerCount = await db.Banners.CountAsync(),
                ProjectCount = await db.Projects.CountAsync(),
            };
            return View(avm);
        }
    }
}
