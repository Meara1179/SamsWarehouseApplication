using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SamsWarehouseApplication.Models
{
    public class AppUserDTO
    {
        [Display(Name = "Email")]
        public string UserEmail { get; set; }

        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string UserPassword { get; set; }
    }
}
