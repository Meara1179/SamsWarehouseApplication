using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SamsWarehouseApplication.Models
{
    public class AppUser
    {
        [Column("AppUserId")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int AppUserId { get; set; }

        [Column("UserEmail")]
        [Required]
        [StringLength(200)]
        [Display(Name = "Email")]
        public string UserEmail { get; set; }

        [Column("UserPasswordHash")]
        [Required]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string UserPasswordHash { get; set; }

        [Column("UserRole")]
        [Required]
        public string AppUserRole { get; set; }

        public List<ShoppingList> UserShoppingList { get; set; }
    }
}
