using CTQuanLyTiecCuoi.Entities;
using CTQuanLyTiecCuoi.Entities.Responses;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTQuanLyTiecCuoi.Repositories
{
    class CustomerRepository
    {
        private readonly string _connectionString;

        public CustomerRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public string ConnectionString { get { return _connectionString; } }

        protected internal Customer GetCustomerById(string id)
        {
            try
            {
                var cmd = "SELECT * FROM customers WHERE id = @id";
                var reader = SqlHelper.ExecuteReader(_connectionString, CommandType.Text, cmd, new SqlParameter("@id", id));
                if (reader.Read())
                {
                    return new Customer()
                    {
                        Id = reader["id"].ToString(),
                        Fullname = reader["fullname"].ToString(),
                        Email = reader["email"].ToString(),
                        Address = reader["address"].ToString(),
                        Phone = reader["phone"].ToString(),
                        NIN = reader["nin"].ToString()
                    };
                }
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public CustomerResponse GetCustomerByPhone(string phone)
        {
            try
            {
                var cmd = "SELECT * FROM customers WHERE phone = @phone";
                var reader = SqlHelper.ExecuteReader(_connectionString, CommandType.Text, cmd, new SqlParameter("@phone", phone));
                if (reader.Read())
                {
                    var res = new Customer()
                    {
                        Id = reader["id"].ToString(),
                        Fullname = reader["fullname"].ToString(),
                        Email = reader["email"].ToString(),
                        Address = reader["address"].ToString(),
                        Phone = reader["phone"].ToString(),
                        NIN = reader["nin"].ToString()
                    };

                    return new CustomerResponse(true, "Fetch data successfully", res);
                }
                return new CustomerResponse(false, "Failed to fetch data");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public CustomerResponse InsertCustomer(Customer customer)
        {
            try
            {
                var cmd = "INSERT INTO customers (fullname, phone, email, address, nin) "
                    + "VALUES(@fullname, @phone, @email, @address, @nin)";
                var parameters = new SqlParameter[]
                {
                    new SqlParameter("@fullname", SqlDbType.NVarChar) { Value = customer.Fullname },
                    new SqlParameter("@phone", SqlDbType.Char) { Value = customer.Phone },
                    new SqlParameter("@email", SqlDbType.NVarChar) { Value = customer.Email },
                    new SqlParameter("@address", SqlDbType.NVarChar) { Value = customer.Address },
                    new SqlParameter("@nin", SqlDbType.Char) { Value = customer.NIN }
                };

                int res = SqlHelper.ExecuteNonQuery(_connectionString, CommandType.Text, cmd, parameters);

                return new CustomerResponse(true, "Insert successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new CustomerResponse(false, ex.Message);
            }
        }

        public CustomerResponse UpdateCustomer(Customer customer)
        {
            if (customer == null) return new CustomerResponse(false, "Invalid data");
            if (string.IsNullOrEmpty(customer.Id)) return new CustomerResponse(false, "Id can not be null");

            try
            {
                var cmd = "UPDATE customers SET [fullname] = @fullname, phone = @phone, [email]=@email, address=@address, nin=@nin WHERE id = @id";

                var parameters = new SqlParameter[]
                {
                    new SqlParameter("@fullname", SqlDbType.NVarChar) { Value = customer.Fullname },
                    new SqlParameter("@phone", SqlDbType.Char) { Value = customer.Phone },
                    new SqlParameter("@email", SqlDbType.NVarChar) { Value = customer.Email },
                    new SqlParameter("@address", SqlDbType.NVarChar) { Value = customer.Address },
                    new SqlParameter("@nin", SqlDbType.Char) { Value = customer.NIN },
                    new SqlParameter("@id", SqlDbType.VarChar) { Value = customer.Id }
                };

                int res = SqlHelper.ExecuteNonQuery(_connectionString, CommandType.Text, cmd, parameters);

                return new CustomerResponse(true, "Update successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new CustomerResponse(false, "Update failed");
            }
        }
    }
}
