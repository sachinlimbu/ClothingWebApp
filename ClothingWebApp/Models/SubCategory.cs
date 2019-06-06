﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothingWebApp.Models
{
    public class SubCategory
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Category Name")]
        [Required]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public Category Category { get; set; }
    }
}
