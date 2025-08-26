using ItecwebApp.Models;

namespace ItecwebApp.Interfaces
{

  
        public interface IUserDAL
        {
            bool RegisterUser(User user);
            User GetUserByUsername(string username);
        User? GetUserByEmail(string email);
        List<User> GetAllUsers();
        bool Updateuser(User user);
        public bool DeleteUser(int userId);
        List<User> searchusers(string text);
        User? GetUserById(int userId);
        }
    
}
