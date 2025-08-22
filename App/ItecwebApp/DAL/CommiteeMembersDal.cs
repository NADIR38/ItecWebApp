using ItecwebApp.Interfaces;
using ItecwebApp.Models;
using MySql.Data.MySqlClient;

namespace ItecwebApp.DAL
{
    public class CommiteeMembersDal : ICommiteeMembersDal
    {
        public bool AddMembers(CommitteMemeber m)
        {
            try
            {
                int commitee_id = DatabaseHelper.getcommiteeid(m.CommiteeName);
                int role_id = DatabaseHelper.lookup_id(m.role_name);
                using (var conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();
                    string query = @"insert into committee_members (name,committee_id,role_id)values(@name,@comm,@role)";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@name", m.name);
                        cmd.Parameters.AddWithValue("@comm", commitee_id);
                        cmd.Parameters.AddWithValue("@role", role_id);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("errow while adding" + ex.Message);
            }
        }
        public List<CommitteMemeber> GetMembers(string commiteeName)
        {
            try
            {
                List<CommitteMemeber> members = new List<CommitteMemeber>();
                using (var conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();
                    string query = @"select cm.member_id,cm.name,cm.role_id,r.value,c.committee_name from committee_members cm inner join lookup r on cm.role_id=r.lookup_id join committees c on c.committee_id=cm.committee_id";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@comm", commiteeName);
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                CommitteMemeber m = new CommitteMemeber
                                {
                                    Id = reader.GetInt32("member_id"),
                                    name = reader.GetString("name"),
                                    role_name = reader.GetString("value"),
                                    CommiteeName = reader.GetString("committee_name")
                                };
                                members.Add(m);
                            }
                        }
                    }
                }
                return members;
            }
            catch (Exception ex)
            {
                throw new Exception("errow while getting members" + ex.Message);
            }
        }
        public List<CommitteMemeber> getallmembers()
        {
            try
            {
                List<CommitteMemeber> members = new List<CommitteMemeber>();
                using (var conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();
                    string query = @"select cm.member_id,cm.name,cm.role_id,r.value,c.committee_name from committee_members cm inner join lookup r on cm.role_id=r.lookup_id join committees c on c.committee_id=cm.committee_id";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                CommitteMemeber m = new CommitteMemeber
                                {
                                    Id = reader.GetInt32("member_id"),
                                    name = reader.GetString("name"),
                                    role_name = reader.GetString("value"),
                                    CommiteeName = reader.GetString("committee_name")
                                };
                                members.Add(m);
                            }
                        }
                    }
                }
                return members;
            }
            catch (Exception ex)
            {
                throw new Exception("errow while getting all members" + ex.Message);
            }
        }
        public List<CommitteMemeber> searchmembers(string searchTerm)
        {
            try
            {
                List<CommitteMemeber> members = new List<CommitteMemeber>();
                using (var conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();
                    string query = @"select cm.member_id,cm.name,cm.role_id,r.value,c.committee_name from committee_members cm inner join lookup r on cm.role_id=r.lookup_id join committees c on c.committee_id=cm.committee_id where cm.name like @searchTerm or c.committee_name like @searchTerm";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@searchTerm", "%" + searchTerm + "%");
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                CommitteMemeber m = new CommitteMemeber
                                {
                                    Id = reader.GetInt32("member_id"),
                                    name = reader.GetString("name"),
                                    role_name = reader.GetString("value"),
                                    CommiteeName = reader.GetString("committee_name")
                                };
                                members.Add(m);
                            }
                        }
                    }
                }
                return members;
            }
            catch (Exception ex)
            {
                throw new Exception("errow while searching members" + ex.Message);
            }
        }

    }
}
