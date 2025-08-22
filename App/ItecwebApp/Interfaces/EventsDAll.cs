using ItecwebApp.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace ItecwebApp.DAL
{
    public class EventsDAll : IEventsDAll
    {

        public bool AddEvent(Events e)
        {
            int itec_id = DatabaseHelper.getitecid(e.year);
            int venue_id = DatabaseHelper.getvenueid(e.venue_name);
            int committee_id = DatabaseHelper.getcommiteeid(e.committee_name);

            try
            {
                using (var conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();
                    using (var trans = conn.BeginTransaction())
                    {
                        // Step 1: Get category_id if exists, otherwise insert
                        int category_id = GetOrCreateCategoryId(e.category_name, conn, trans);

                        // Step 2: Insert event
                        string queryEvent = @"
                            INSERT INTO itec_events 
                                (event_name, description, event_date, venue_id, committee_id, itec_id, event_category_id) 
                            VALUES 
                                (@event_name, @event_description, @event_date, @venue_id, @committee_id, @itec_id, @category_id)";
                        using (var cmdEvent = new MySqlCommand(queryEvent, conn, trans))
                        {
                            cmdEvent.Parameters.AddWithValue("@event_name", e.event_name);
                            cmdEvent.Parameters.AddWithValue("@event_description", e.event_description);
                            cmdEvent.Parameters.AddWithValue("@event_date", e.event_date);
                            cmdEvent.Parameters.AddWithValue("@venue_id", venue_id);
                            cmdEvent.Parameters.AddWithValue("@committee_id", committee_id);
                            cmdEvent.Parameters.AddWithValue("@itec_id", itec_id);
                            cmdEvent.Parameters.AddWithValue("@category_id", category_id);

                            cmdEvent.ExecuteNonQuery();
                            e.event_id = Convert.ToInt32(cmdEvent.LastInsertedId);
                        }

                        // Step 3: Insert venue allocation
                        string queryAlloc = @"
                            INSERT INTO venue_allocations (event_id, venue_id, assigned_date, assigned_time) 
                            VALUES (@event, @venue, @date, @time)";
                        using (var cmdAlloc = new MySqlCommand(queryAlloc, conn, trans))
                        {
                            cmdAlloc.Parameters.AddWithValue("@event", e.event_id);
                            cmdAlloc.Parameters.AddWithValue("@venue", venue_id);
                            cmdAlloc.Parameters.AddWithValue("@date", e.event_date);
                            cmdAlloc.Parameters.AddWithValue("@time", e.assigned_time.TimeOfDay);
                            cmdAlloc.ExecuteNonQuery();
                        }

                        trans.Commit();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding event: " + ex.Message);
            }
        }

        public bool updatevent(Events e)
        {
            int itec_id = DatabaseHelper.getitecid(e.year);
            int venue_id = DatabaseHelper.getvenueid(e.venue_name);
            int committee_id = DatabaseHelper.getcommiteeid(e.committee_name);

            try
            {
                using (var conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();
                    using (var trans = conn.BeginTransaction())
                    {
                        // Step 1: Get category_id if exists, otherwise insert
                        int category_id = GetOrCreateCategoryId(e.category_name, conn, trans);

                        // Step 2: Update event
                        string queryEvent = @"
                            UPDATE itec_events 
                            SET event_name=@event,
                                venue_id=@venue, 
                                committee_id=@committee,
                                event_date=@date,
                                itec_id=@itec
                                
                            WHERE event_id=@id";
                        using (var cmdEvent = new MySqlCommand(queryEvent, conn, trans))
                        {
                            cmdEvent.Parameters.AddWithValue("@event", e.event_name);
                            cmdEvent.Parameters.AddWithValue("@venue", venue_id);
                            cmdEvent.Parameters.AddWithValue("@committee", committee_id);
                            cmdEvent.Parameters.AddWithValue("@date", e.event_date);
                            cmdEvent.Parameters.AddWithValue("@itec", itec_id);
                            cmdEvent.Parameters.AddWithValue("@id", e.event_id);
                            cmdEvent.ExecuteNonQuery();
                        }

                        // Step 3: Update venue allocation
                        string queryAlloc = @"
                            UPDATE venue_allocations 
                            SET assigned_date=@date,
                                venue_id=@venue 
                            WHERE event_id=@id";
                        using (var cmdAlloc = new MySqlCommand(queryAlloc, conn, trans))
                        {
                            cmdAlloc.Parameters.AddWithValue("@date", e.event_date);
                            cmdAlloc.Parameters.AddWithValue("@venue", venue_id);
                            cmdAlloc.Parameters.AddWithValue("@id", e.event_id);
                            cmdAlloc.ExecuteNonQuery();
                        }

                        trans.Commit();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating event: " + ex.Message);
            }
        }

        // Helper: get category_id or insert if new
        private int GetOrCreateCategoryId(string categoryName, MySqlConnection conn, MySqlTransaction trans)
        {
            int category_id = 0;
            string checkQuery = "SELECT event_category_id FROM event_categories WHERE category_name=@category_name LIMIT 1";
            using (var cmdCheck = new MySqlCommand(checkQuery, conn, trans))
            {
                cmdCheck.Parameters.AddWithValue("@category_name", categoryName);
                var result = cmdCheck.ExecuteScalar();
                if (result != null)
                    category_id = Convert.ToInt32(result);
                else
                {
                    string insertQuery = "INSERT INTO event_categories (category_name) VALUES (@category_name)";
                    using (var cmdInsert = new MySqlCommand(insertQuery, conn, trans))
                    {
                        cmdInsert.Parameters.AddWithValue("@category_name", categoryName);
                        cmdInsert.ExecuteNonQuery();
                        category_id = Convert.ToInt32(cmdInsert.LastInsertedId);
                    }
                }
            }
            return category_id;
        }
    


public List<Events> GetEvents(string searchTerm = "")
        {
            var list = new List<Events>();

            try
            {
                using (var conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();

                    string query = @"
                        SELECT 
                            i.event_id,
                            i.event_name,
                            i.event_date,
                            i.description,
                            e.year,
                            v.venue_name,
                            c.category_name,
                            ca.committee_name,
                            va.assigned_time
                        FROM itec_events i
                        LEFT JOIN venue_allocations va ON i.event_id = va.event_id
                        LEFT JOIN venues v ON v.venue_id = va.venue_id
                        LEFT JOIN itec_editions e ON i.itec_id = e.itec_id
                        LEFT JOIN event_categories c ON c.event_category_id = i.event_category_id
                        LEFT JOIN committees ca ON ca.committee_id = i.committee_id
                        WHERE (@search = '' 
                               OR i.event_name LIKE CONCAT('%', @search, '%') 
                               OR v.venue_name LIKE CONCAT('%', @search, '%') 
                               OR ca.committee_name LIKE CONCAT('%', @search, '%'))
                        GROUP BY i.event_id, i.event_name, i.event_date, i.description, e.year, v.venue_name, c.category_name, ca.committee_name, va.assigned_time";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@search", searchTerm);

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int event_id = reader.GetInt32("event_id");
                                string event_name = reader.GetString("event_name");
                                DateTime event_date = reader.GetDateTime("event_date");
                                string event_description = reader.GetString("description");
                                int year = reader.GetInt32("year");
                                string venue_name = reader.GetString("venue_name");
                                string category_name = reader.GetString("category_name");
                                string committee_name = reader.GetString("committee_name");

                                // Convert TIME (TimeSpan) from DB to DateTime (today's date + time)
                                DateTime assigned_time = DateTime.MinValue;
                                if (reader["assigned_time"] != DBNull.Value)
                                {
                                    TimeSpan ts = (TimeSpan)reader["assigned_time"];
                                    assigned_time = DateTime.Today.Add(ts);
                                }

                                Events e = new Events(
                                    event_id,
                                    event_name,
                                    event_description,
                                    event_date,
                                    venue_name,
                                    committee_name,
                                    year,
                                    category_name,
                                    assigned_time
                                );

                                list.Add(e);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching events: " + ex.Message);
            }

            return list;
        }
    }
}
