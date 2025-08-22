using ItecwebApp.Interfaces;
using ItecwebApp.Models;

namespace ItecwebApp.DAL
{
    public class VendorsDAl : IVendorsDAl
    {
        public List<Vendors> GetVendors()
        {
            try
            {
                using (var conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "SELECT * FROM Vendors";
                        using (var reader = cmd.ExecuteReader())
                        {
                            var vendors = new List<Vendors>();
                            while (reader.Read())
                            {
                                var vendor = new Vendors
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("vendor_id")),
                                    Name = reader.GetString(reader.GetOrdinal("vendor_name")),
                                    ContactPerson = reader.GetString(reader.GetOrdinal("contact")),
                                    ServiceType = reader.GetString(reader.GetOrdinal("service_type"))
                                };
                                vendors.Add(vendor);
                            }
                            return vendors;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving vendors.", ex);
            }
        }

        public bool AddVendor(Vendors v)
        {
            try
            {
                using (var conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "INSERT INTO Vendors (vendor_name, contact, service_type) VALUES (@Name, @ContactPerson, @ServiceType)";
                        cmd.Parameters.AddWithValue("@Name", v.Name);
                        cmd.Parameters.AddWithValue("@ContactPerson", v.ContactPerson);
                        cmd.Parameters.AddWithValue("@ServiceType", v.ServiceType);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding the vendor.", ex);
            }
        }
        public bool EditVendor(Vendors v)
        {
            try
            {
                using (var conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "UPDATE Vendors SET vendor_name = @Name, contact = @ContactPerson, service_type = @ServiceType WHERE vendor_id = @Id";
                        cmd.Parameters.AddWithValue("@Id", v.Id);
                        cmd.Parameters.AddWithValue("@Name", v.Name);
                        cmd.Parameters.AddWithValue("@ContactPerson", v.ContactPerson);
                        cmd.Parameters.AddWithValue("@ServiceType", v.ServiceType);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while editing the vendor.", ex);

            }

        }
        public List<Vendors> Search(string term)
        {
            try
            {
                using (var conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "SELECT * FROM Vendors WHERE vendor_name LIKE @Term OR contact LIKE @Term OR service_type LIKE @Term";
                        cmd.Parameters.AddWithValue("@Term", "%" + term + "%");

                        using (var reader = cmd.ExecuteReader())
                        {
                            var vendors = new List<Vendors>();
                            while (reader.Read())
                            {
                                var vendor = new Vendors
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("vendor_id")),
                                    Name = reader.GetString(reader.GetOrdinal("vendor_name")),
                                    ContactPerson = reader.GetString(reader.GetOrdinal("contact")),
                                    ServiceType = reader.GetString(reader.GetOrdinal("service_type"))
                                };
                                vendors.Add(vendor);
                            }
                            return vendors;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while searching for vendors.", ex);
            }
        }

    }
}
