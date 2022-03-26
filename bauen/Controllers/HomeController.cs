using bauen.DAL;
using bauen.HomeViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace bauen.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext db;
        public HomeController(AppDbContext _db)
        {
            db = _db;
        }
        public async Task<IActionResult> Index()
        {
            HomeViewModel hvm = new HomeViewModel()
            {
                Banners = await db.Banners.ToListAsync(),
                AboutUs=await db.Abouts.FirstOrDefaultAsync(),
                Projects=await db.Projects.ToListAsync(),
            };
            return View(hvm);
        }
    }
}
