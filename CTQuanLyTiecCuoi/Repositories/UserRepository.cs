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
    class UserRepository
    {
        private readonly string _connectionString;
        private Session _session;

        public UserRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public string ConnectionString { get { return _connectionString; } }
        public Session Session { get { return _session; } }

        private Account GetAccountByUsername(string username)
        {
            var cmd = "SELECT * FROM Accounts WHERE username=@username";
            SqlParameter phoneParam = new SqlParameter("@username", SqlDbType.VarChar) { Value = username };
            // using var res = SqlHelper.ExecuteReader(_connectionString, CommandType.Text, cmd, phoneParam);
            try
            {
                var res = SqlHelper.ExecuteReader(_connectionString, CommandType.Text, cmd, phoneParam);
                if (res.HasRows)
                {
                    res.Read();
                    return new Account
                    {
                        Id = res["id"].ToString(),
                        Username = res["username"].ToString(),
                        HashPassword = res["hash_password"].ToString()
                    };
                }
            }
            catch { throw; }

            return null;
        }

        private int AddAccount(Account user)
        {
            try
            {
                if (string.IsNullOrEmpty(user.Username) || string.IsNullOrEmpty(user.Password))
                {
                    return -1;
                }

                var cmd = "INSERT INTO accounts(username, hash_password) VALUES(@username, @hash)";
                SqlParameter[] sqlParams = new SqlParameter[] {
                    new SqlParameter("@username", SqlDbType.VarChar) { Value = user.Username },
                    new SqlParameter("@hash", SqlDbType.NVarChar) { Value = PasswordHashOMatic.Hash(user.Password) }                    
                };

                return SqlHelper.ExecuteNonQuery(_connectionString, CommandType.Text, cmd, sqlParams);
            }
            catch { throw; }

        }

        // login method
        public UserResponse Login(string username, string password)
        {
            try
            {
                var storedUser = GetAccountByUsername(username);
                if (storedUser == null)
                {
                    return new UserResponse(false, "No account found. Please check the phone number.");
                }
                if (!PasswordHashOMatic.Verify(password, storedUser.HashPassword))
                {
                    return new UserResponse(false, "The password provided is not valid");
                }

                _session = new Session(storedUser.Id, storedUser.Username, DateTime.Now);

                return new UserResponse(true, "Login successfully", storedUser);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new UserResponse(false, ex.Message);
            }
        }

        // register method
        public UserResponse Register(Account user)
        {
            try
            {
                int res = AddAccount(user);
                if (res == -1) return new UserResponse(false, "Please fill all the required fields");
                if (res == 0) return new UserResponse(false, "Account registration failed");

                
                Account newUser = GetAccountByUsername(user.Username);
                _session = new Session(newUser.Id, newUser.Username, DateTime.Now);

                return new UserResponse(true, "Register successfully", newUser);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new UserResponse(false, "An error occurred");
            }
        }

        public UserResponse Logout()
        {
            _session = null;
            return new UserResponse(true, "Logout successfully");
        }

    }
}
