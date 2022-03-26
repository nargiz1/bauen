using bauen.DAL;
using bauen.Models;
using bauen.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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
    public class ProjectController : Controller
    {
        private readonly AppDbContext db;
        private readonly IWebHostEnvironment env;
        public ProjectController(AppDbContext _db, IWebHostEnvironment _env)
        {
            db = _db;
            env = _env;
        }
        public async Task<IActionResult> Index()
        {
            return View(await db.Projects.ToListAsync());
        }
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(Project project)
        {
            if (!ModelState.IsValid) return View();
            Project duplicate = await db.Projects.FirstOrDefaultAsync(x => x.Id == project.Id);
            if (duplicate != null) return View();
            project.Image = await project.ImageFile.Upload(env.WebRootPath, @"img/projects");
            await db.Projects.AddAsync(project);
            await db.SaveChangesAsync();
            if (project.ImageFiles.Count != 0)
            {
                foreach (IFormFile item in project.ImageFiles)
                {
                    ProjectImage pImg = new ProjectImage()
                    {
                        Image = await item.Upload(env.WebRootPath, @"img/projects"),
                        ProjectId = project.Id,
                    };
                    db.ProjectImages.Add(pImg);
                }
                await db.SaveChangesAsync();
            }
            return RedirectToAction("Index", "Project");
        }
        public async Task<IActionResult> Info(int? id)
        {
            if (id == null) return RedirectToAction("Index", "Project");
            return View(await db.Projects.Include(x => x.ProjectImages).FirstOrDefaultAsync(x => x.Id == id));
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return RedirectToAction("Index", "Project");
            return View(await db.Projects.Include(x => x.ProjectImages).FirstOrDefaultAsync(x => x.Id == id));
        }

        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            if (id == null) return RedirectToAction("Index", "Project");
            Project ProjectToDelete = await db.Projects.FindAsync(id);
            if (ProjectToDelete == null) return RedirectToAction("Index", "Project");
            db.Projects.Remove(ProjectToDelete);
            await db.SaveChangesAsync();
            return RedirectToAction("Index", "Project");
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return RedirectToAction("Index", "Project");
            return View(await db.Projects.Include(x => x.ProjectImages).FirstOrDefaultAsync(x => x.Id == id));
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Project project)
        {
            if (!ModelState.IsValid) return View();
            if (project.ImageFile != null)
            {
                project.Image = await project.ImageFile.Upload(env.WebRootPath, @"img/projects");
            }
            db.Projects.Update(project);
            await db.SaveChangesAsync();
            return RedirectToAction("Index", "Project");
        }
    }
}
