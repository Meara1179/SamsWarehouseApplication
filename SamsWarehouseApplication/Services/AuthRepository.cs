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

        /// <summary>
        /// Calls the GetUserByyUserName method to get the appropriate user, then encrypts the supplied password using BCrypt and compares it against the hashed 
        /// password stored on the database.
        /// </summary>
        /// <param name="credentials"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Compares the supplied email with the emails stored on the database, then returns the matching user.
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        private AppUser GetUserByUserName(string email) 
        {
            var user = _context.AppUsers.Where(x => x.UserEmail.Equals(email)).FirstOrDefault();
            return user;
        }
    }
}
