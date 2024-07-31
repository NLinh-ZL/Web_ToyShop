using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToyShop.Models
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ProductID { get; set; }
        [Required (ErrorMessage = "Hãy nhập tên sản phẩm")]
        public string ProductName { get; set; }
        [Required (ErrorMessage = "Hãy nhập giá của sản phẩm")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Chỉ nhập số")]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int ProductPrice { get; set; }
        [Required(ErrorMessage = "Hãy nhập mô tả sản phẩm")]
        public string ProductDescription { get; set; }
        [Required(ErrorMessage = "Hãy chọn loại sản phẩm")]
        public long CategoryID { get; set; }
        [Required(ErrorMessage = "Hãy chọn thương hiệu của sản phẩm")]
        public long BrandID { get; set; }
        public virtual ICollection<Image> Images { get; set; }
        public virtual Brand Brand { get; set; }
        public virtual Category Category { get; set; }
    }
}