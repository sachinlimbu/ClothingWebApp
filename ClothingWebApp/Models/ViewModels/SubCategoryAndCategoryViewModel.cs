﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothingWebApp.Models.ViewModels
{
    public class SubCategoryAndCategoryViewModel
    {
        public IEnumerable<Category> Categorieslist { get; set; }
        public SubCategory SubCategory { get; set; }
        public List<String> SubCategoryList { get; set; }
        public string StatusMessage { get; set; }
    }
}
