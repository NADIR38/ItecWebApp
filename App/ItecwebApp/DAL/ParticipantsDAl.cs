using ItecwebApp.Interfaces;
using ItecwebApp.Models;
using MySql.Data.MySqlClient;

namespace ItecwebApp.DAL
{
    public class ParticipantsDAL : IParticipantsDAL
    {
        public bool AddParticipant(Participants participant)
        {
            int itec_id = DatabaseHelper.getitecid(participant.year);
            int event_id = DatabaseHelper.geteventid(participant.event_name);
            int role_id = DatabaseHelper.getroleid(participant.role);
            int payment_status_id = DatabaseHelper.lookup_id(participant.payment_status);

            try
            {
                using (var conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();
                    using (var trans = conn.BeginTransaction())
                    {
                        long participantId;

                        string query1 = "INSERT INTO participants (name, email, role_id, contact, institute, itec_id) " +
                                       "VALUES (@Name, @Email, @Role, @Phone, @Institute, @ItecId)";
                        using (var cmd = new MySqlCommand(query1, conn, trans))
                        {
                            cmd.Parameters.AddWithValue("@Name", participant.Name);
                            cmd.Parameters.AddWithValue("@Email", participant.Email);
                            cmd.Parameters.AddWithValue("@Role", role_id);
                            cmd.Parameters.AddWithValue("@Phone", participant.phone ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@Institute", participant.institute ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@ItecId", itec_id);

                            cmd.ExecuteNonQuery();
                            participantId = cmd.LastInsertedId;
                        }

                        string query2 = "INSERT INTO event_participants (participant_id, event_id, payment_status_id, fee_amount) " +
                                       "VALUES (@ParticipantId, @EventId, @PaymentStatus, @FeeAmount)";
                        using (var cmd = new MySqlCommand(query2, conn, trans))
                        {
                            cmd.Parameters.AddWithValue("@ParticipantId", participantId);
                            cmd.Parameters.AddWithValue("@EventId", event_id);
                            cmd.Parameters.AddWithValue("@PaymentStatus", payment_status_id);
                            cmd.Parameters.AddWithValue("@FeeAmount", participant.feeamount);
                            cmd.ExecuteNonQuery();
                        }

                        trans.Commit();
                        return true;
                    }
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Database error: " + ex.Message);
                return false;
            }
        }

        public List<Participants> GetAllParticipants()
        {
            try
            {
                using (var conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT p.participant_id, p.name, p.email, r.role_name AS role, p.contact AS phone, " +
                                   "p.institute, i.year, e.event_name, ps.value AS payment_status, ep.fee_amount " +
                                   "FROM participants p " +
                                   "JOIN roles r ON p.role_id = r.role_id " +
                                   "JOIN itec_editions i ON p.itec_id = i.itec_id " +
                                   "JOIN event_participants ep ON p.participant_id = ep.participant_id " +
                                   "JOIN itec_events e ON ep.event_id = e.event_id " +
                                   "JOIN lookup ps ON ep.payment_status_id = ps.lookup_id";

                    using (var cmd = new MySqlCommand(query, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        var participants = new List<Participants>();
                        while (reader.Read())
                        {
                            var participant = new Participants
                            {
                                Id = reader.GetInt32("participant_id"),
                                Name = reader.GetString("name"),
                                Email = reader.GetString("email"),
                                role = reader.GetString("role"),
                                phone = reader.IsDBNull(reader.GetOrdinal("phone")) ? null : reader.GetString("phone"),
                                institute = reader.IsDBNull(reader.GetOrdinal("institute")) ? null : reader.GetString("institute"),
                                year = reader.GetInt32("year"),
                                event_name = reader.GetString("event_name"),
                                payment_status = reader.GetString("payment_status"),
                                feeamount = reader.GetDecimal("fee_amount")
                            };
                            participants.Add(participant);
                        }
                        return participants;
                    }
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Database error: " + ex.Message);
                return new List<Participants>();
            }
        }

        public List<Participants> SearchParticipants(string searchTerm)
        {
            try
            {
                using (var conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT p.participant_id, p.name, p.email, r.role_name AS role, p.contact AS phone, " +
                                   "p.institute, i.year, e.event_name, ps.value AS payment_status, ep.fee_amount " +
                                   "FROM participants p " +
                                   "JOIN roles r ON p.role_id = r.role_id " +
                                   "JOIN itec_editions i ON p.itec_id = i.itec_id " +
                                   "JOIN event_participants ep ON p.participant_id = ep.participant_id " +
                                   "JOIN itec_events e ON ep.event_id = e.event_id " +
                                   "JOIN lookup ps ON ep.payment_status_id = ps.lookup_id " +
                                   "WHERE p.name LIKE @Search OR p.email LIKE @Search OR e.event_name LIKE @Search";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Search", "%" + searchTerm + "%");

                        using (var reader = cmd.ExecuteReader())
                        {
                            var participants = new List<Participants>();
                            while (reader.Read())
                            {
                                var participant = new Participants
                                {
                                    Id = reader.GetInt32("participant_id"),
                                    Name = reader.GetString("name"),
                                    Email = reader.GetString("email"),
                                    role = reader.GetString("role"),
                                    phone = reader.IsDBNull(reader.GetOrdinal("phone")) ? null : reader.GetString("phone"),
                                    institute = reader.IsDBNull(reader.GetOrdinal("institute")) ? null : reader.GetString("institute"),
                                    year = reader.GetInt32("year"),
                                    event_name = reader.GetString("event_name"),
                                    payment_status = reader.GetString("payment_status"),
                                    feeamount = reader.GetDecimal("fee_amount")
                                };
                                participants.Add(participant);
                            }
                            return participants;
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Database error: " + ex.Message);
                return new List<Participants>();
            }
        }

        public bool UpdateParticipant(Participants participant)
        {
            try
            {
                int role_id = DatabaseHelper.getroleid(participant.role);
                int itec_id = DatabaseHelper.getitecid(participant.year);
                int event_id = DatabaseHelper.geteventid(participant.event_name);
                int payment_status_id = DatabaseHelper.lookup_id(participant.payment_status);

                using (var conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();
                    using (var trans = conn.BeginTransaction())
                    {
                        string query1 = "UPDATE participants SET name=@Name, email=@Email, role_id=@Role, " +
                                       "contact=@Phone, institute=@Institute, itec_id=@ItecId WHERE participant_id=@Id";
                        using (var cmd = new MySqlCommand(query1, conn, trans))
                        {
                            cmd.Parameters.AddWithValue("@Id", participant.Id);
                            cmd.Parameters.AddWithValue("@Name", participant.Name);
                            cmd.Parameters.AddWithValue("@Email", participant.Email);
                            cmd.Parameters.AddWithValue("@Role", role_id);
                            cmd.Parameters.AddWithValue("@Phone", participant.phone ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@Institute", participant.institute ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@ItecId", itec_id);
                            cmd.ExecuteNonQuery();
                        }

                        string query2 = "UPDATE event_participants SET event_id=@EventId, payment_status_id=@PaymentStatus, fee_amount=@FeeAmount " +
                                       "WHERE participant_id=@ParticipantId";
                        using (var cmd = new MySqlCommand(query2, conn, trans))
                        {
                            cmd.Parameters.AddWithValue("@ParticipantId", participant.Id);
                            cmd.Parameters.AddWithValue("@EventId", event_id);
                            cmd.Parameters.AddWithValue("@PaymentStatus", payment_status_id);
                            cmd.Parameters.AddWithValue("@FeeAmount", participant.feeamount);
                            cmd.ExecuteNonQuery();
                        }

                        trans.Commit();
                        return true;
                    }
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Database error: " + ex.Message);
                return false;
            }
        }

        public bool DeleteParticipant(int id)
        {
            try
            {
                using (var conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();
                    using (var trans = conn.BeginTransaction())
                    {
                        string query1 = "DELETE FROM event_participants WHERE participant_id=@Id";
                        using (var cmd = new MySqlCommand(query1, conn, trans))
                        {
                            cmd.Parameters.AddWithValue("@Id", id);
                            cmd.ExecuteNonQuery();
                        }

                        string query2 = "DELETE FROM participants WHERE participant_id=@Id";
                        using (var cmd = new MySqlCommand(query2, conn, trans))
                        {
                            cmd.Parameters.AddWithValue("@Id", id);
                            cmd.ExecuteNonQuery();
                        }

                        trans.Commit();
                        return true;
                    }
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Database error: " + ex.Message);
                return false;
            }
        }
    }
}
