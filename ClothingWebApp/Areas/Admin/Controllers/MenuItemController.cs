using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ClothingWebApp.Data;
using ClothingWebApp.Models;
using ClothingWebApp.Models.ViewModels;
using ClothingWebApp.Utility;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ClothingWebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MenuItemController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly IHostingEnvironment hostingEnvironment;

        [BindProperty]
        public MenuItemViewModel MenuItemViewModel { get; set; }

        public MenuItemController(ApplicationDbContext context, IHostingEnvironment hostingEnvironment)
        {
            this.context = context;
            this.hostingEnvironment = hostingEnvironment;
            MenuItemViewModel = new MenuItemViewModel()
            {
                MenuItem = new Models.MenuItem(),
                Categories = context.Categories
                //SubCategories = context.SubCategories,//This depends on which subcategories is selected 

            };
        }

        //GET INDEX
        public async Task<IActionResult> Index()
        {
            var attactchconxt = await context.MenuItems.Include(x => x.SubCategory).Include(x => x.Category).ToListAsync();

            if (attactchconxt == null)
            {
                return NotFound();
            }

            return View(attactchconxt);
        }

        //GET CREATE
        public IActionResult Create()
        {
            if (MenuItemViewModel == null)
            {
                return NotFound();
            }
            return View(MenuItemViewModel);
        }


        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PostCreate()
        {
            //Assign Sub Category Post because it's been assigned using Javascript and no model has been passed
            MenuItemViewModel.MenuItem.SubCategoryId = Convert.ToInt32(Request.Form["SubCategoryId"].ToString());

            if (!ModelState.IsValid)
            {
                return View(MenuItemViewModel);
            }

            context.MenuItems.Add(MenuItemViewModel.MenuItem);
            await context.SaveChangesAsync();

            //Work on the image saving section

            //Extra root Path and for that you need IHostingEnvironment
            string webRootPath = hostingEnvironment.WebRootPath;
            //Extract
            var files = HttpContext.Request.Form.Files;

            var MenuItemFromDb = await context.MenuItems.FindAsync(MenuItemViewModel.MenuItem.Id);
            if (files.Count > 0)
            {
                //Files has been upload
                var uploads = Path.Combine(webRootPath, "Images");
                var extension = Path.GetExtension(files[0].FileName);

                using (var filesStream = new FileStream(Path.Combine(uploads, MenuItemViewModel.MenuItem.Id + extension), FileMode.Create))
                {
                    files[0].CopyTo(filesStream);
                }
                MenuItemFromDb.Image = @"\Images\" + MenuItemViewModel.MenuItem.Id + extension;
            }
            else
            {
                //No files was uploaded, so use default image
                var uploads = Path.Combine(webRootPath, @"Images\" + SD.DefaultFoodImage);
                System.IO.File.Copy(uploads, webRootPath + @"\Images\" + MenuItemViewModel.MenuItem.Id + ".png");
                MenuItemFromDb.Image = @"\Images\" + MenuItemViewModel.MenuItem.Id + ".png";

            }
            await context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        //GET - Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //Retrieve the menu item
            MenuItemViewModel.MenuItem = await context.MenuItems
                .Include(x => x.Category)
                .Include(x => x.SubCategory)
                .SingleOrDefaultAsync(x => x.Id == id);
            //Load Sub Category
            MenuItemViewModel.SubCategories = await context.SubCategories
                .Where(s => s.CategoryId == MenuItemViewModel.MenuItem.CategoryId)
                .ToListAsync();
            if (MenuItemViewModel.MenuItem == null)
            {
                return NotFound();
            }
            return View(MenuItemViewModel);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PostEdit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            //Assign Sub Category Post because it's been assigned using Javascript and no model has been passed
            MenuItemViewModel.MenuItem.SubCategoryId = Convert.ToInt32(Request.Form["SubCategoryId"].ToString());

            if (!ModelState.IsValid)
            {
                //We populate before sending back
                MenuItemViewModel.SubCategories = await context.SubCategories
                    .Where(s => s.CategoryId == MenuItemViewModel.MenuItem.CategoryId).ToListAsync();
                return View(MenuItemViewModel);
            }

            //context.MenuItems.Add(MenuItemViewModel.MenuItem);
            //await context.SaveChangesAsync();

            //Work on the image saving section

            //Extra root Path and for that you need IHostingEnvironment
            string WebRootPath = hostingEnvironment.WebRootPath;
            //Extract
            var files = HttpContext.Request.Form.Files;

            var MenuItemFromDb = await context.MenuItems.FindAsync(MenuItemViewModel.MenuItem.Id);
            if (files.Count > 0)
            {
                //Files has been upload
                var uploads = Path.Combine(WebRootPath, "Images");
                var extension = Path.GetExtension(files[0].FileName);

                //Delete the original file therefore New file can be pasted
                var imagePath = Path.Combine(WebRootPath, MenuItemFromDb.Image.TrimStart('\\'));
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
                //We will upload a new file                
                using (var filesStream = new FileStream(Path.Combine(uploads, MenuItemViewModel.MenuItem.Id + extension), FileMode.Create))
                {
                    files[0].CopyTo(filesStream);
                }
                MenuItemFromDb.Image = @"\Images\" + MenuItemViewModel.MenuItem.Id + extension;
            }

            //If image has not been upload we are going to do nothing


            MenuItemFromDb.Name = MenuItemViewModel.MenuItem.Name;
            MenuItemFromDb.Description = MenuItemViewModel.MenuItem.Description;
            MenuItemFromDb.Price = MenuItemViewModel.MenuItem.Price;
            MenuItemFromDb.ItemWeight = MenuItemViewModel.MenuItem.ItemWeight;
            MenuItemFromDb.CategoryId = MenuItemViewModel.MenuItem.CategoryId;
            MenuItemFromDb.SubCategoryId = MenuItemViewModel.MenuItem.SubCategoryId;

            await context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        //GET - DETAILS

        public async Task<IActionResult> Details(int ? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //Retrieve the menu item
            MenuItemViewModel.MenuItem = await context.MenuItems
                .Include(x => x.Category)
                .Include(x => x.SubCategory)
                .SingleOrDefaultAsync(x => x.Id == id);
            //Load Sub Category
            MenuItemViewModel.SubCategories = await context.SubCategories
                .Where(s => s.CategoryId == MenuItemViewModel.MenuItem.CategoryId)
                .ToListAsync();
            if (MenuItemViewModel.MenuItem == null)
            {
                return NotFound();
            }
            return View(MenuItemViewModel);
        }


        //GET - DELETE
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //Retrieve the menu item
            MenuItemViewModel.MenuItem = await context.MenuItems
                .Include(x => x.Category)
                .Include(x => x.SubCategory)
                .SingleOrDefaultAsync(x => x.Id == id);
            //Load Sub Category
            MenuItemViewModel.SubCategories = await context.SubCategories
                .Where(s => s.CategoryId == MenuItemViewModel.MenuItem.CategoryId)
                .ToListAsync();
            if (MenuItemViewModel.MenuItem == null)
            {
                return NotFound();
            }
            return View(MenuItemViewModel);
        }


        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PostDelete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            string webRootPath = hostingEnvironment.WebRootPath;
            MenuItem menuItem = await context.MenuItems.FindAsync(id);
            if (menuItem != null)
            {
                var imagePath = Path.Combine(webRootPath, menuItem.Image.TrimStart('\\'));

                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
                context.MenuItems.Remove(menuItem);
                await context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
        //GET Sub Category
        public async Task<IActionResult> GetSubCategory(int id)
        {
            List<SubCategory> subCategories = new List<SubCategory>();
            subCategories = await (from item in context.SubCategories
                                   where item.CategoryId == id
                                   select item).ToListAsync();
            return Json(new SelectList(subCategories, "Id", "Name"));
        }
    }
}