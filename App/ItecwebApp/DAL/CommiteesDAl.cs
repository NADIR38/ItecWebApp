using ItecwebApp.Interfaces;
using ItecwebApp.Models;
using MySql.Data.MySqlClient;
using System.Security.Cryptography.X509Certificates;

namespace ItecwebApp.DAL
{
    public class CommiteesDAl : ICommiteesDAl
    {
        public bool AddCommitee(Models.Commitees c)
        {
            try
            {
                int itecid = DatabaseHelper.getitecid(c.year);
                using (var conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();
                    string query = "INSERT INTO committees (committee_name, itec_id) VALUES (@name, @itec_id)";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@name", c.Name);
                        cmd.Parameters.AddWithValue("@itec_id", itecid);
                        return cmd.ExecuteNonQuery() > 0;
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding commitee", ex);

            }

        }

    }
}
