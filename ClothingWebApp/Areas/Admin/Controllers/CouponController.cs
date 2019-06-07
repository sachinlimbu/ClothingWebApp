using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ClothingWebApp.Data;
using ClothingWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClothingWebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CouponController : Controller
    {
        private readonly ApplicationDbContext context;

        [BindProperty]
        public Coupon Coupon { get; set; }

        public CouponController(ApplicationDbContext context)
        {
            this.context = context;

        }
        public async Task<IActionResult> Index()
        {
            var returnFromDb = await context.coupons.ToListAsync();
            return View(returnFromDb);
        }

        public IActionResult Create()
        {
            return View();
        }


        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PostCreate(int? id)
        {
            if (ModelState.IsValid)
            {
                //Fletch the file
                var files = HttpContext.Request.Form.Files;
                if (files.Count > 0)
                {
                    //files was uploaded

                    //Convert to stream of byte
                    byte[] p1 = null;
                    using (var fs1 = files[0].OpenReadStream())//will start reading the file
                    {
                        using (var ms1 = new MemoryStream())
                        {
                            fs1.CopyTo(ms1);
                            //Convert into stream of byte and store into p1
                            p1 = ms1.ToArray();
                        }
                        Coupon.Picture = p1;
                    }
                }
                context.coupons.Add(Coupon);
                await context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(Coupon);
        }

        public async Task<IActionResult> Edit(int?id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Coupon = await context.coupons.FirstOrDefaultAsync(m => m.Id == id);
            if (Coupon == null)
            {
                return NotFound();
            }
            return View(Coupon);
        }

    }
}