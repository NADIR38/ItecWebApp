using ItecwebApp.Models;

namespace ItecwebApp.Interfaces
{

  
        public interface IUserDAL
        {
            bool RegisterUser(User user);
            User GetUserByUsername(string username);
        }
    
}
