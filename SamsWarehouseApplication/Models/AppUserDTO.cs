using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SamsWarehouseApplication.Models
{
    public class AppUserDTO
    {
        public string UserEmail { get; set; }

        public string UserPassword { get; set; }
    }
}
