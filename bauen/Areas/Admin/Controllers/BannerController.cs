using bauen.DAL;
using bauen.Models;
using bauen.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace bauen.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles="Admin")]
    public class BannerController : Controller
    {
        private readonly AppDbContext db;
        private readonly IWebHostEnvironment env;
        public BannerController(AppDbContext _db, IWebHostEnvironment _env)
        {
            db = _db;
            env = _env;
        }
        public async Task<IActionResult> Index()
        {
            return View(await db.Banners.ToListAsync());
        }
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(Banner banner)
        {
            if (!ModelState.IsValid) return View();
            Project duplicate = await db.Projects.FirstOrDefaultAsync(x => x.Id == banner.Id);
            if (duplicate != null) return View();
            banner.Image = await banner.ImageFile.Upload(env.WebRootPath, @"img/slider");
            await db.Banners.AddAsync(banner);
            await db.SaveChangesAsync();
            return RedirectToAction("Index", "Banner");
        }
        public async Task<IActionResult> Info(int? id)
        {
            if (id == null) return RedirectToAction("Index", "Banner");
            return View(await db.Banners.FirstOrDefaultAsync(x => x.Id == id));
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return RedirectToAction("Index", "Banner");
            return View(await db.Banners.FirstOrDefaultAsync(x => x.Id == id));
        }
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            if (id == null) return RedirectToAction("Index", "Banner");
            Banner BannerToDelete = await db.Banners.FindAsync(id);
            if (BannerToDelete == null) return RedirectToAction("Index", "Banner");
            db.Banners.Remove(BannerToDelete);
            await db.SaveChangesAsync();
            return RedirectToAction("Index", "Banner");
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return RedirectToAction("Index", "Banner");
            return View(await db.Banners.FirstOrDefaultAsync(x => x.Id == id));
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Banner banner)
        {
            if (!ModelState.IsValid) return View();
            if (banner.ImageFile != null)
            {
                banner.Image = await banner.ImageFile.Upload(env.WebRootPath, @"img/slider");
            }
            db.Banners.Update(banner);
            await db.SaveChangesAsync();
            return RedirectToAction("Index", "Banner");
        }
    }
}
