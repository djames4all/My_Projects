using System.ComponentModel.DataAnnotations;

namespace Prog7311_POE_Part2.Models
{
    public class Product
    {

        // ProductID is the Unique Key
        public int ProductId { get; set; }


        // ProductName

        [Required(ErrorMessage ="Product Name Is Required")]
        [StringLength(100, ErrorMessage = "Product Name cannot exceed 100 characters")]
        public string ProductName { get; set; }


        // ProductDescription 
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string ProductDescription { get; set; }


        // ProductCategory
        [Required(ErrorMessage = "Product Category Is Required")]
        [StringLength(50, ErrorMessage = "Category cannot exceed 50 characters")]
        public string ProductCategory { get; set; }


        // ProductionDate
        [Required(ErrorMessage = "Product Date Is Required")]
        public DateTime ProductionDate { get; set; }


        // ProductQuantity
        [RegularExpression(@"^[1-9]\d*$", 
            ErrorMessage = "Quantity must be a whole number greater than zero")]
        public string ProductQuantity { get; set; }


        // ProductUnitPrice
        [RegularExpression(@"^(?!0+(\.0{1,2})?$)\d+(\.\d{1,2})?$", 
            ErrorMessage = "Unit Price must be a positive number with up to two decimal places")]
        public string ProductUnitPrice { get; set; }

        // UserID
        public string? CreatedByUserID { get; set; }

    }
}
