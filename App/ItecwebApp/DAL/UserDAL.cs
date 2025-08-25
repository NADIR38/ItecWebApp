using ItecwebApp.Interfaces;
using ItecwebApp.Models;
using MySql.Data.MySqlClient;

namespace ItecwebApp.DAL
{
    public class UserDAL : IUserDAL
    {
        public bool RegisterUser(User user)
        {
            int role_id=DatabaseHelper.getroleid(user.role_name);
            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                string query = "INSERT INTO users (username, password_hash, email, role_id,name) " +
                               "VALUES (@username, @password, @email, @roleId,@name)";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@username", user.Username);
                    cmd.Parameters.AddWithValue("@password", user.Password_Hash); // Already hashed in controller
                    cmd.Parameters.AddWithValue("@email", user.Email);
                    cmd.Parameters.AddWithValue("@roleId", role_id);
                    cmd.Parameters.AddWithValue("@name", user.Name);

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public User GetUserByUsername(string username)
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                string query = "SELECT u.user_id, u.username, u.password_hash, u.email, u.role_id, u.name,r.role_name " +
                               "FROM users u join roles r on r.role_id=u.role_id WHERE u.username = @username";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@username", username);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new User
                            {
                                User_Id = reader.GetInt32("user_id"),
                                Username = reader.GetString("username"),
                                Password_Hash = reader.GetString("password_hash"),
                                Email = reader.GetString("email"),
                                Role_Id = reader.GetInt32("role_id"),
                                Name = reader.GetString("name"),
                                role_name = reader.GetString("role_name")
                            };
                        }
                    }
                }
            }
            return null;
        }
        // Remove ValidateUser method - not needed since we use PasswordHelper in controller
        // Remove HashPassword method - we use PasswordHelper consistently
    }
}