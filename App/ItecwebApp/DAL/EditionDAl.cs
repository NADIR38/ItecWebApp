using ItecwebApp.Interfaces;
using ItecwebApp.Models;
using MySql.Data.MySqlClient;

namespace ItecwebApp.DAL
{
    public class EditionDAl : IEditionDAl
    {
        public bool AddEdition(Edition e)
        {
            try
            {
                using (var conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();
                    string query = "INSERT INTO itec_editions (Year, Theme, Description) VALUES (@Year, @Theme, @Description)";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Year", e.Year);
                        cmd.Parameters.AddWithValue("@Theme", e.Theme);
                        cmd.Parameters.AddWithValue("@Description", e.Description);

                        return cmd.ExecuteNonQuery() > 0;
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding edition", ex);
            }
        }
        public List<Edition> GetEditions()
        {
            var list = new List<Edition>();
            try
            {

                using (var conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();
                    string query = "select * from itec_editions";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int id = reader.GetInt32("itec_id");
                                int year = reader.GetInt32("Year");
                                string theme = reader.GetString("Theme");
                                string description = reader.GetString("Description");
                                var edition = new Edition(id, year, theme, description);
                                list.Add(edition);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving editions", ex);
            }
            return list;
        }
    }
}