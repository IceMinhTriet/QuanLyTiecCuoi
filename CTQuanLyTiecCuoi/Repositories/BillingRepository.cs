using CTQuanLyTiecCuoi.Entities;
using CTQuanLyTiecCuoi.Entities.Responses;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTQuanLyTiecCuoi.Repositories
{
    internal class BillingRepository
    {
        private readonly string _connectionString;
        //readonly HallRepository _hallRepository;
        //readonly CustomerRepository _customerRepository;

        public BillingRepository(string connectionString)
        {
            _connectionString = connectionString;
            //_hallRepository = new HallRepository(_connectionString);
            //_customerRepository = new CustomerRepository(_connectionString);
        }

        public string ConnectionString { get { return _connectionString; } }

        public BillingResponse GetBillingByReservationId(string reserv_id)
        {
            try
            {
                var cmd = "SELECT * FROM billing WHERE reservation_id=@reserv_id";
                SqlParameter param = new SqlParameter("@reserv_id", SqlDbType.VarChar) { Value = reserv_id };
                var reader = SqlHelper.ExecuteReader(_connectionString, CommandType.Text, cmd, param);
                if (reader.Read())
                {
                    Billing b = new Billing()
                    {
                        Id = reader["id"].ToString(),
                        HallCost = Convert.ToDecimal(reader["hall_cost"]),
                        ServiceCost = Convert.ToDecimal(reader["service_cost"]),
                        FoodCost = Convert.ToDecimal(reader["food_cost"]),
                        TotalCost = Convert.ToDecimal(reader["total_cost"]),
                        Deposit = Convert.ToDecimal(reader["deposit"]),
                        RemainingCost = Convert.ToDecimal(reader["remaining_cost"]),
                        Note = reader["note"].ToString(),
                        IsDone = Convert.ToBoolean(reader["is_done"]),
                        PaymentDate = DateTime.Parse(reader["payment_date"].ToString(), CultureInfo.InvariantCulture),
                        StaffId = reader["account_id"].ToString()

                    };
                    return new BillingResponse(true, "Fetch data successfully", b);
                }

                return new BillingResponse(false, "Failed to fetch data");
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new BillingResponse(false, ex.Message);
            }
        }

        public BillingResponse UpdateBillingStatus(string reserv_id, string account_id)
        {
            try
            {
                var updateBillCmd = "UPDATE billing SET is_done=@status, payment_date=@date, account_id=@account_id WHERE reservation_id=@reserv_id";
                var upadateReservCmd = "UPDATE reservations SET status='completed' WHERE id=@reserv_id";
                SqlParameter reservIdParam = new SqlParameter("@reserv_id", SqlDbType.VarChar) { Value = reserv_id };
                SqlParameter statusParam = new SqlParameter("@status", SqlDbType.Bit) { Value = 1 };
                SqlParameter paymentDateParam = new SqlParameter("@date", SqlDbType.SmallDateTime) { Value = DateTime.Now };
                SqlParameter accountParam = new SqlParameter("@account_id", SqlDbType.VarChar) { Value = account_id };

                SqlHelper.ExecuteNonQuery(_connectionString, CommandType.Text, updateBillCmd, 
                    new SqlParameter[] {
                    statusParam, paymentDateParam, reservIdParam, accountParam
                });

                SqlHelper.ExecuteNonQuery(_connectionString, CommandType.Text, upadateReservCmd, reservIdParam);
                return new BillingResponse(true, "Update bill successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new BillingResponse(false, ex.Message);
            }
        }
    }
}
