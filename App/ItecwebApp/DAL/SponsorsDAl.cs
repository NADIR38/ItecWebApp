using ItecwebApp.Interfaces;
using ItecwebApp.Models;
using MySql.Data.MySqlClient;

namespace ItecwebApp.DAL
{
    public class SponsorsDAl : ISponsorsDAl
    {
        public bool AddSponsor(Sponsors sponsor)
        {
            try
            {
                using (var coonn = DatabaseHelper.GetConnection())
                {
                    coonn.Open();
                    var query = "INSERT INTO Sponsors (sponsor_name, contact) VALUES (@SponsorName, @ContactNo)";
                    using (var cmd = new MySqlCommand(query, coonn))
                    {
                        cmd.Parameters.AddWithValue("@SponsorName", sponsor.SponsorName);
                        cmd.Parameters.AddWithValue("@ContactNo", sponsor.ContactNo);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding sponsor: " + ex.Message, ex);
            }

        }
        public List<Sponsors> GetSponsors()
        {
            var sponsors = new List<Sponsors>();
            try
            {
                using (var conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();
                    var query = "SELECT * FROM Sponsors";
                    using (var cmd = new MySqlCommand(query, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            sponsors.Add(new Sponsors
                            {
                                SponsorId = reader.GetInt32("sponsor_id"),
                                SponsorName = reader.GetString("sponsor_name"),
                                ContactNo = reader.GetString("contact")
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving sponsors: " + ex.Message, ex);
            }
            return sponsors;
        }

        public bool UpdateSponsor(Sponsors sponsor)
        {
            try
            {
                using (var conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();
                    var query = "UPDATE Sponsors SET sponsor_name = @SponsorName, contact = @ContactNo WHERE sponsor_id = @SponsorId";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@SponsorId", sponsor.SponsorId);
                        cmd.Parameters.AddWithValue("@SponsorName", sponsor.SponsorName);
                        cmd.Parameters.AddWithValue("@ContactNo", sponsor.ContactNo);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating sponsor: " + ex.Message, ex);
            }
        }
        public List<Sponsors> searchsponsors(string searchTerm)
        {
            var sponsors = new List<Sponsors>();
            try
            {
                using (var conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();
                    var query = "SELECT * FROM Sponsors WHERE sponsor_name LIKE @SearchTerm OR contact LIKE @SearchTerm";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@SearchTerm", "%" + searchTerm + "%");
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                sponsors.Add(new Sponsors
                                {
                                    SponsorId = reader.GetInt32("sponsor_id"),
                                    SponsorName = reader.GetString("sponsor_name"),
                                    ContactNo = reader.GetString("contact")
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error searching sponsors: " + ex.Message, ex);
            }
            return sponsors;
        }
    }
}
