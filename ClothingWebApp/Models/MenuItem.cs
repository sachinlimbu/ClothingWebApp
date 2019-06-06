using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothingWebApp.Models
{
    public class MenuItem
    {
        [key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public enum ItemSize
        {
            xxs = 0,
            xs = 1,
            s = 2,
            m = 3,
            xl = 4,
            xxl = 5
        }

        public int ItemWeight { get; set; }

        public string Image { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Price should be greater than ${1}")]
        public double Price { get; set; }

        //Category
        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }

        //SubCategory

        [Display(Name = "SubCategoryId")]
        public int SubCategoryId { get; set; }

        [ForeignKey("SubCategor")]
        public virtual SubCategory SubCategory { get; set; }
    }
}
