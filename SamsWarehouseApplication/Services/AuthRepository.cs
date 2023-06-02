using SamsWarehouseApplication.Models;
using System.Linq;

namespace SamsWarehouseApplication.Services
{
    public class AuthRepository
    {
        private readonly ShoppingContext _context;
        
        public AuthRepository(ShoppingContext context)
        {
            _context = context;
        }

        public AppUser Authenticate(AppUserDTO credentials)
        {
            var userDetails = GetUserByUserName(credentials.UserEmail);

            if (userDetails == null)
            {
                return null;
            }

            if (BCrypt.Net.BCrypt.EnhancedVerify(credentials.UserPassword, userDetails.UserPasswordHash))
            {
                return userDetails;
            }
            return null;
        }

        private AppUser GetUserByUserName(string email) 
        {
            var user = _context.AppUsers.Where(x => x.UserEmail.Equals(email)).FirstOrDefault();
            return user;
        }
    }
}
