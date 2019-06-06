using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClothingWebApp.Data;
using ClothingWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClothingWebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext context;

        [BindProperty]
        public Category Category { get; set; }

        public CategoryController(ApplicationDbContext context)
        {
            this.context = context;
        }

        //GET : CATEGORY
        public async Task<IActionResult> Index()
        {
            var returnCategory = await context.Categories.ToListAsync();
            return View(returnCategory);
        }

        //GET : CREATE
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost, ActionName("create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PostCreate()
        {
            if (ModelState.IsValid)
            {
                var createCategory = context.Categories.Add(Category);
                await context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(Category);
        }

        //GET : EDIT
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Category = await context.Categories.FindAsync(id);
            if (Category == null)
            {
                return NotFound();
            }
            return View(Category);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        //POST: EDIT
        public async Task<IActionResult> PostEdit(int? id)
        {
            if (ModelState.IsValid)
            {
                context.Categories.Update(Category);
                await context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(Category);
        }

        //GET : DETAILS
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Category = await context.Categories.FindAsync(id);

            if (Category == null)
            {
                return NotFound();
            }
            return View(Category);
        }

        //GET DELETE
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Category = await context.Categories.FindAsync(id);
            if (Category == null)
            {
                return NotFound();
            }
            return View(Category);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> PostDelete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                context.Remove(Category);
                await context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(Category);
        }
    }
}