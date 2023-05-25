using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SamsWarehouseApplication.Models
{
    public class ShoppingList
    {
        [Column("ShoppingListId")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int ShoppingListId { get; set; }

        [Column("ShoppingListName")]
        [Required]
        [StringLength(200)]
        public string ShoppingListName { get; set;}

        [Column("AppUserId")]
        [Required]
        public int AppUserId { get; set; }

        [Required]
        public DateTime ShoppingListDate { get; set; }

        public AppUser User { get; set; }

        public List<ShoppingListItem> ShoppingListItems { get; set; }
    }
}
