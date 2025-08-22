using ItecwebApp.Interfaces;
using ItecwebApp.Models;
using MySql.Data.MySqlClient;

namespace ItecwebApp.DAL
{
    public class VenueDAl : IVenueDAl
    {
        public bool addvenue(Venues v)
        {
            try
            {
                using (var conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();
                    string query = "INSERT INTO venues (venue_name, location, capacity) VALUES (@name, @location, @capacity)";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@name", v.name);
                        cmd.Parameters.AddWithValue("@location", v.location);
                        cmd.Parameters.AddWithValue("@capacity", v.capacity);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Error adding venue", ex);
            }
        }
        public List<Venues> GetVenues()
        {
            var list = new List<Venues>();
            try
            {
                using (var conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT * FROM venues";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int id = reader.GetInt32("venue_id");
                                string name = reader.GetString("venue_name");
                                string location = reader.GetString("location");
                                int capacity = reader.GetInt32("capacity");
                                var venue = new Venues(id, name, location, capacity);
                                list.Add(venue);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving venues", ex);
            }
            return list;
        }
        public List<string> GetVenueNames(string name)
        {
            var list = new List<string>();
            try
            {
                using (var conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT venue_name FROM venues WHERE venue_name LIKE @name";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@name", "%" + name + "%");
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                list.Add(reader.GetString("venue_name"));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving venue names", ex);
            }
            return list;
        }
        public bool EditVenue(Venues v)
        {
            try
            {
                using (var conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();
                    string query = "UPDATE venues SET venue_name = @name, location = @location, capacity = @capacity WHERE venue_id = @id";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", v.id);
                        cmd.Parameters.AddWithValue("@name", v.name);
                        cmd.Parameters.AddWithValue("@location", v.location);
                        cmd.Parameters.AddWithValue("@capacity", v.capacity);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating venue", ex);
            }
        }
public List<Venues> searchvenues(string term)
        {
            var list = new List<Venues>();
            try
            {
                using (var conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT * FROM venues WHERE venue_name LIKE @term or location=@term";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@term", "%" + term + "%");
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int id = reader.GetInt32("venue_id");
                                string name = reader.GetString("venue_name");
                                string location = reader.GetString("location");
                                int capacity = reader.GetInt32("capacity");
                                var venue = new Venues(id, name, location, capacity);
                                list.Add(venue);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error searching venues", ex);
            }
            return list;
        }
    }
}