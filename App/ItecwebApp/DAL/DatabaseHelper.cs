using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Linq;

public static class DatabaseHelper
{
    private static string _connectionString;

    public static void Init(string connectionString)
    {
        _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
    }

    public static MySqlConnection GetConnection()
    {
        if (string.IsNullOrEmpty(_connectionString))
            throw new InvalidOperationException("DatabaseHelper not initialized with a connection string.");

        return new MySqlConnection(_connectionString);
    }

    public static MySqlDataReader ExecuteReader(string query, MySqlParameter[] parameters = null)
    {
        var conn = GetConnection();
        conn.Open();
        var cmd = new MySqlCommand(query, conn);
        if (parameters != null)
            cmd.Parameters.AddRange(parameters);
        return cmd.ExecuteReader(CommandBehavior.CloseConnection);
    }

    public static int ExecuteNonQuery(string query, MySqlParameter[] parameters = null)
    {
        using var conn = GetConnection();
        conn.Open();
        using var cmd = new MySqlCommand(query, conn);
        if (parameters != null)
            cmd.Parameters.AddRange(parameters);
        return cmd.ExecuteNonQuery();
    }

    public static object ExecuteScalar(string query, MySqlParameter[] parameters = null)
    {
        using var conn = GetConnection();
        conn.Open();
        using var cmd = new MySqlCommand(query, conn);
        if (parameters != null)
            cmd.Parameters.AddRange(parameters);
        return cmd.ExecuteScalar();
    }
    public static MySqlParameter[] CreateMySqlParameters(Dictionary<string, object> parameterDict)
    {
        return parameterDict
                    .Select(p => new MySqlParameter(p.Key, p.Value ?? DBNull.Value))
                    .ToArray();
    }
    public static List<string> getvenuenames(string name)
    {
        var list = new List<string>();
        try
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                string query = "SELECT venue_name FROM itec_venues WHERE venue_name LIKE @name";
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
    public static int getitecid(int year)
    {
        try
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                string query = "select itec_id from itec_editions where year=@year";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@year", year);
                    object result = cmd.ExecuteScalar();
                    return result != null ? Convert.ToInt32(result) : -1;

                }
            }
        }
        catch (Exception e)
        {
            throw new Exception("error retreving itec_id" + e.Message);
        }
    }
    public static int getcommiteeid(string name)
    {
        try
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                string query = "select committee_id from committees where  committee_name=@name";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@name", name);
                    object result = cmd.ExecuteScalar();
                    return result != null ? Convert.ToInt32(result) : -1;
                }
            }
        }
        catch (Exception e)
        {
            throw new Exception("error retreving commitee_id" + e.Message);
        }
    }
    public static List<string> getcommittenames(string term)
    {
        var list = new List<string>();
        try
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                string query = "SELECT committee_name FROM committees WHERE committee_name LIKE @term";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@term", "%" + term + "%");
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(reader.GetString("committee_name"));
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Error retrieving committee names", ex);
        }
        return list;
    }
    public static int getvenueid(string name)
    {
        try
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                string query = "select venue_id from venues where venue_name=@name";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@name", name);
                    object result = cmd.ExecuteScalar();
                    return result != null ? Convert.ToInt32(result) : -1;
                }
            }
        }
        catch (Exception e)
        {
            throw new Exception("error retreving venue_id" + e.Message);
        }
    }
    public static int committeeid(string name)
    {
        try
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                string query = "select committee_id from committees where committee_name=@name";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@name", name);
                    object result = cmd.ExecuteScalar();
                    return result != null ? Convert.ToInt32(result) : -1;
                }
            }
        }
        catch (Exception e)
        {
            throw new Exception("error retreving committee_id" + e.Message);
        }

    }
    public static int getcategoryid(string name)
    {
        try
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                string query = "select event_category_id from event_categories where category_name=@name";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@name", name);
                    object result = cmd.ExecuteScalar();
                    return result != null ? Convert.ToInt32(result) : -1;
                }
            }
        }
        catch (Exception e)
        {
            throw new Exception("error retreving category_id" + e.Message);
        }
    }
    public static List<string> getcategorynames()
    {
        var list = new List<string>();
        try
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                string query = "SELECT category_name FROM event_categories ";
                using (var cmd = new MySqlCommand(query, conn))
                {

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(reader.GetString("category_name"));
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Error retrieving category names", ex);
        }
        return list;
    }
    public static int geteventid(string name)
    {
        try
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                string query = "select event_id from itec_events where event_name=@name";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@name", name);
                    object result = cmd.ExecuteScalar();
                    return result != null ? Convert.ToInt32(result) : -1;
                }
            }
        }
        catch (Exception e)
        {
            throw new Exception("error retreving event_id" + e.Message);
        }
    }
    public static List<string> geteventnames(string term)
    {
        var list = new List<string>();
        try
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                string query = "SELECT event_name FROM itec_events WHERE event_name LIKE @term";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@term", "%" + term + "%");
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(reader.GetString("event_name"));
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Error retrieving event names", ex);
        }
        return list;
    }
    public static List<string> getlookup(string category)
    {
        var list = new List<string>();
        try
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                string query = "SELECT value from lookup where category=@category ";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@category", category);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(reader.GetString("value"));
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Error retrieving committee names", ex);
        }
        return list;
    }
    public static int lookup_id(string value)
    {
        try
        {

            using (var conn = GetConnection())
            {
                conn.Open();
                string query = "select lookup_id from lookup where value=@name";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@name", value);
                    object result = cmd.ExecuteScalar();
                    return result != null ? Convert.ToInt32(result) : -1;
                }
            }
        }
        catch (Exception e)
        {
            throw new Exception("error retreving event_id" + e.Message);
        }


    }

    public static int getroleid(string role)
    {
        try
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                string query = "select role_id from roles where role_name=@role";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@role", role);
                    object result = cmd.ExecuteScalar();
                    return result != null ? Convert.ToInt32(result) : -1;
                }
            }
        }
        catch (Exception e)
        {
            throw new Exception("error retreving role_id" + e.Message);
        }
    }
}

