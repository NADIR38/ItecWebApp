using ItecwebApp.Interfaces;
using ItecwebApp.Models;
using MySql.Data.MySqlClient;

namespace ItecwebApp.DAL
{
    public class DutiesDAl : IDutiesDAl
    {
        public bool assign_duty(Duties d)
        {
            try
            {
                int statusId = DatabaseHelper.lookup_id(d.status);
                int committeeId = DatabaseHelper.getcommiteeid(d.committee_name);
                using (var conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();
                    var query = "INSERT INTO Duties (committee_id, assigned_to, task_description, deadline, status_id) VALUES (@CommitteeName, @Name, @Description, @Deadline, @Status)";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@CommitteeName", committeeId);
                        cmd.Parameters.AddWithValue("@Name", d.name);
                        cmd.Parameters.AddWithValue("@Description", d.description);
                        cmd.Parameters.AddWithValue("@Deadline", d.Deadline);
                        cmd.Parameters.AddWithValue("@Status", statusId);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error assigning duty: " + ex.Message, ex);
            }
        }
        public List<Duties> getduties()
        {
            try
            {
                using (var conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();
                    var query = @"SELECT d.duty_id, c.committee_name, d.assigned_to, d.task_description, d.deadline, l.value AS status 
                                  FROM Duties d 
                                  INNER JOIN Committees c ON d.committee_id = c.committee_id 
                                  INNER JOIN Lookup l ON d.status_id = l.lookup_id";
                    using (var cmd = new MySqlCommand(query, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        var dutiesList = new List<Duties>();
                        while (reader.Read())
                        {
                            var duty = new Duties
                            {
                                duty_id = reader.GetInt32("duty_id"),
                                committee_name = reader.GetString("committee_name"),
                                name = reader.GetString("assigned_to"),
                                description = reader.GetString("task_description"),
                                Deadline = reader.GetDateTime("deadline"),
                                status = reader.GetString("status")
                            };
                            dutiesList.Add(duty);
                        }
                        return dutiesList;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving duties: " + ex.Message, ex);
            }
        }
        public List<Duties> search(string term)
        {
            try
            {
                using (var conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();
                    var query = @"SELECT d.duty_id, c.committee_name, d.assigned_to, d.task_description, d.deadline, l.value AS status 
                                  FROM Duties d 
                                  INNER JOIN Committees c ON d.committee_id = c.committee_id 
                                  INNER JOIN Lookup l ON d.status_id = l.lookup_id 
                                      WHERE d.assigned_to LIKE @term OR d.task_description LIKE @term OR c.committee_name LIKE @term ";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@term", "%" + term + "%");

                        using (var reader = cmd.ExecuteReader())
                        {
                            var dutiesList = new List<Duties>();
                            while (reader.Read())
                            {
                                var duty = new Duties
                                {
                                    duty_id = reader.GetInt32("duty_id"),
                                    committee_name = reader.GetString("committee_name"),
                                    name = reader.GetString("assigned_to"),
                                    description = reader.GetString("task_description"),
                                    Deadline = reader.GetDateTime("deadline"),
                                    status = reader.GetString("status")
                                };
                                dutiesList.Add(duty);
                            }
                            return dutiesList;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving duties: " + ex.Message, ex);
            }

        }
        public bool Updatestatus(Duties d)
        {
            try
            {
                int statusId = DatabaseHelper.lookup_id(d.status);
                using (var conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();
                    var query = "UPDATE Duties SET status_id = @Status WHERE duty_id = @Id";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Status", statusId);
                        cmd.Parameters.AddWithValue("@Id", d.duty_id);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating status: " + ex.Message, ex);
            }
        }
    }
}
