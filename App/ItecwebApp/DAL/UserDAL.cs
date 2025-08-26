using ItecwebApp.Interfaces;
using ItecwebApp.Models;
using MySql.Data.MySqlClient;
using System.Data;

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
        public User? GetUserByEmail(string email)
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                string query = "SELECT u.user_id, u.username, u.password_hash, u.email, u.role_id, u.name, r.role_name " +
                               "FROM users u JOIN roles r ON r.role_id = u.role_id WHERE u.email = @Email";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Email", email);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new User
                            {
                                User_Id = reader.GetInt32("user_id"),
                                Username = reader.GetString("username"),
                                Password_Hash = reader.IsDBNull("password_hash") ? null : reader.GetString("password_hash"),
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
        public List<User> GetAllUsers()
        {
            var users = new List<User>();
            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                string query = "SELECT u.user_id, u.username, u.password_hash, u.email, u.role_id, u.name, r.role_name " +
                               "FROM users u JOIN roles r ON r.role_id = u.role_id";
                using (var cmd = new MySqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        users.Add(new User
                        {
                            User_Id = reader.GetInt32("user_id"),
                            Username = reader.GetString("username"),
                            Password_Hash = reader.IsDBNull("password_hash") ? null : reader.GetString("password_hash"),
                            Email = reader.GetString("email"),
                            Role_Id = reader.GetInt32("role_id"),
                            Name = reader.GetString("name"),
                            role_name = reader.GetString("role_name")
                        });
                    }
                }
            }
            return users;
        }

        public bool Updateuser(User user)
        {
            int role_id = DatabaseHelper.getroleid(user.role_name);
            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();

                string query;
                if (!string.IsNullOrEmpty(user.Password_Hash))
                {
                    query = @"UPDATE users 
                      SET username = @username, email = @Email, role_id = @roleId, name = @name, password_hash = @password 
                      WHERE user_id = @userId";
                }
                else
                {
                    query = @"UPDATE users 
                      SET username = @username, email = @Email, role_id = @roleId, name = @name 
                      WHERE user_id = @userId";
                }

                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@username", user.Username);
                    cmd.Parameters.AddWithValue("@Email", user.Email);
                    cmd.Parameters.AddWithValue("@roleId", role_id);
                    cmd.Parameters.AddWithValue("@name", user.Name);
                    cmd.Parameters.AddWithValue("@userId", user.User_Id);

                    if (!string.IsNullOrEmpty(user.Password_Hash))
                    {
                        cmd.Parameters.AddWithValue("@password", user.Password_Hash);
                    }

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool DeleteUser(int userId)
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                string query = "DELETE FROM users WHERE user_id = @userId";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@userId", userId);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }
        public User? GetUserById(int userId)
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                string query = "SELECT u.user_id, u.username, u.password_hash, u.email, u.role_id, u.name, r.role_name " +
                               "FROM users u JOIN roles r ON r.role_id = u.role_id WHERE u.user_id = @userId";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@userId", userId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new User
                            {
                                User_Id = reader.GetInt32("user_id"),
                                Username = reader.GetString("username"),
                                Password_Hash = reader.IsDBNull("password_hash") ? null : reader.GetString("password_hash"),
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

        public List<User> searchusers(string text)
        {
            var users = new List<User>();

            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();

                string query = @"
            SELECT u.user_id, u.username, u.password_hash, u.email, u.role_id, u.name, r.role_name
            FROM users u
            JOIN roles r ON r.role_id = u.role_id
            WHERE u.name LIKE @text
               OR u.username LIKE @text
               OR u.email LIKE @text
               OR r.role_name LIKE @text";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@text", "%" + text + "%");

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            users.Add(new User
                            {
                                User_Id = reader.GetInt32("user_id"),
                                Username = reader.GetString("username"),
                                Password_Hash = reader.IsDBNull("password_hash") ? null : reader.GetString("password_hash"),
                                Email = reader.GetString("email"),
                                Role_Id = reader.GetInt32("role_id"),
                                Name = reader.GetString("name"),
                                role_name = reader.GetString("role_name")
                            });
                        }
                    }
                }
            }

            return users;
        }

    }
}