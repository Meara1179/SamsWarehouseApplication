using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SamsWarehouseApplication.Models
{
    public class ShoppingListItem
    {
        [Column("ShoppingListItemId")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int ShoppingListItemId { get; set; }

        [Column("ShoppingListId")]
        [Required]
        public int ShoppingListId { get; set; }

        [Column("ProductId")]
        [Required]
        public int ProductId { get; set; }

        [Column("Quantity")]
        [Required]
        public int Quantity { get; set; }

        public ShoppingList ShoppingList { get; set; }

        public Product Product { get; set; }
    }
}
