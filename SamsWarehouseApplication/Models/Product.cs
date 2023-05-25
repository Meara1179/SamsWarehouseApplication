using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SamsWarehouseApplication.Models
{
    public class Product
    {
        [Column("ProductId")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int ProductId { get; set; }

        [Display(Name = "Name")]
        [Column("ProductName")]
        [Required]
        [StringLength(200)]
        public string ProductName { get; set; }

        [Display(Name = "Unit")]
        [Column("ProductUnit")]
        [Required]
        [StringLength(200)]
        public string ProductUnit { get; set; }

        [Display(Name = "Price")]
        [Column("ProductPrice")]
        [Required]
        public double ProductPrice { get; set; }

        public List<ShoppingListItem> ProductList { get; set; }
    }
}
