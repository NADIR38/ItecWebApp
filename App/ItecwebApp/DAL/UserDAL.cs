using ItecwebApp.Interfaces;
using ItecwebApp.Models;
using MySql.Data.MySqlClient;

namespace ItecwebApp.DAL
{
    public class UserDAL : IUserDAL
    {
        public bool RegisterUser(User user)
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                string query = "INSERT INTO users (username, password_hash, email, role_id) " +
                               "VALUES (@username, @password, @email, @roleId)";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@username", user.Username);
                    cmd.Parameters.AddWithValue("@password", user.Password_Hash); // Already hashed in controller
                    cmd.Parameters.AddWithValue("@email", user.Email);
                    cmd.Parameters.AddWithValue("@roleId", user.Role_Id);

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public User GetUserByUsername(string username)
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                string query = "SELECT user_id, username, password_hash, email, role_id, is_email_confirmed " +
                               "FROM users WHERE username = @username";
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
                                Is_Email_Confirmed = reader.GetBoolean("is_email_confirmed")
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