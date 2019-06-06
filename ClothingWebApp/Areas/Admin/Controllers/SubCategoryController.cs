using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClothingWebApp.Data;
using ClothingWebApp.Models;
using ClothingWebApp.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ClothingWebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SubCategoryController : Controller
    {
        private readonly ApplicationDbContext context;
        public SubCategoryController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [BindProperty]
        public SubCategoryAndCategoryViewModel VwSubCategory { get; set; }

        [TempData]
        public string TempStatusMessage { get; set; }

        //GET INDEX
        public async Task<IActionResult> Index()
        {
            var GetSubCategory = await context.SubCategories.Include(x => x.Category).ToListAsync();

            return View(GetSubCategory);
        }

        //GET CREATE

        public async Task<IActionResult> Create()
        {
            SubCategoryAndCategoryViewModel viewModel = new SubCategoryAndCategoryViewModel()
            {
                Categorieslist = await context.Categories.ToListAsync(),
                SubCategory = new Models.SubCategory(),
                SubCategoryList = await context.SubCategories.OrderBy(p => p.Name).Select(p => p.Name).Distinct().ToListAsync()
            };
            return View(viewModel);
        }

        //POST CREATE
        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PostCreate(SubCategoryAndCategoryViewModel sbCategoryVm)
        {
            if (ModelState.IsValid)
            {
                //var doesSubCategoryExists = context.SubCategories.Include(s => s.Category)
                //    .Where(s => s.Name == VwSubCategory.SubCategory.Name && s.Id == VwSubCategory.SubCategory.Id);

                var doesSubCategoryExists = context.SubCategories.Include(s => s.Category)
                    .Where(s => s.Name == sbCategoryVm.SubCategory.Name && s.CategoryId == sbCategoryVm.SubCategory.CategoryId);

                if (doesSubCategoryExists.Count() > 0)
                {
                    TempStatusMessage = "Sorry the Category you have enter already exists under our database" + doesSubCategoryExists.First().Category.Name;
                }
                else
                {
                    context.SubCategories.Add(VwSubCategory.SubCategory);
                    await context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            SubCategoryAndCategoryViewModel modelVm = new SubCategoryAndCategoryViewModel()
            {
                Categorieslist = await context.Categories.ToListAsync(),
                SubCategory = new Models.SubCategory(),
                SubCategoryList = await context.SubCategories.OrderBy(p => p.Name).Select(p => p.Name).Distinct().ToListAsync(),
                StatusMessage = TempStatusMessage
            };
            return View(modelVm);
        }

        [ActionName("GetSubCategory")]
        public async Task<IActionResult> GetSubCategory(int id)
        {
            List<SubCategory> subCategories = new List<SubCategory>();
            subCategories = await (from item in context.SubCategories
                                   where item.CategoryId == id
                                   select item).ToListAsync();

            return Json(new SelectList(subCategories, "Id", "Name"));
        }

        //GET Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var subCategory = await context.SubCategories.SingleOrDefaultAsync(m => m.Id == id);
            if (subCategory == null)
            {
                return NotFound();
            }
            SubCategoryAndCategoryViewModel model = new SubCategoryAndCategoryViewModel()
            {
                Categorieslist = await context.Categories.ToListAsync(),
                SubCategory = subCategory,
                SubCategoryList = await context.SubCategories
                .OrderBy(n => n.Name)
                .Select(p => p.Name)
                .Distinct()
                .ToListAsync()
            };
            return View(model);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PostEdit(int? id, SubCategoryAndCategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                var doesSubCategoryExist = context.SubCategories.Include(x => x.Category)
                    .Where(x => x.Name == model.SubCategory.Name && x.CategoryId == model.SubCategory.CategoryId);

                if (doesSubCategoryExist.Count() > 0)
                {
                    TempStatusMessage = "Error: Please try different Name: " + doesSubCategoryExist.First().Name + "Category";
                }
                else
                {
                    if (id == null)
                    {
                        return NotFound();
                    }
                    var subCatfromdb = await context.SubCategories.FindAsync(id);
                    subCatfromdb.Name = model.SubCategory.Name;

                    context.SubCategories.Add(model.SubCategory);
                    await context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(model);
        }

        //GET DETAILS
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var findDbContext = await context.SubCategories.Include(x => x.Category).SingleOrDefaultAsync(m => m.Id == id);

            if (findDbContext == null)
            {
                return NotFound();
            }

            return View(findDbContext);
        }

        //GET DETAILS
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var finddbcontext = await context.SubCategories.Include(x => x.Category).SingleOrDefaultAsync(x => x.Id == id);
            if (finddbcontext == null)
            {
                return NotFound();
            }
            return View(finddbcontext);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PostDelete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var FindDbContext = await context.SubCategories.Include(x => x.Category).FirstOrDefaultAsync(x => x.Id == id);

            if (FindDbContext == null)
            {
                return NotFound();
            }
            else
            {
                context.SubCategories.Remove(FindDbContext);
                await context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
        }
    }
}