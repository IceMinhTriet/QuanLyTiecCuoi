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
    class ReservationRepositoy
    {
        private readonly string _connectionString;
        readonly HallRepository _hallRepository;
        readonly CustomerRepository _customerRepository;

        public ReservationRepositoy(string connectionString)
        {
            _connectionString = connectionString;
            _hallRepository = new HallRepository(_connectionString);
            _customerRepository = new CustomerRepository(_connectionString);
        }

        public string ConnectionString { get { return _connectionString; } }

        private List<Dish> GetDishesByReservationId(string id)
        {
            List<Dish> dishes = new List<Dish>();
            var cmd = "SELECT * FROM dishes WHERE id IN (SELECT dish_id FROM menus WHERE reservation_id=@reservation_id)";
            var idParam = new SqlParameter("@reservation_id", SqlDbType.VarChar) { Value = id };
            var reader = SqlHelper.ExecuteReader(_connectionString, CommandType.Text, cmd, idParam);

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    var dish = new Dish()
                    {
                        Id = reader["id"].ToString(),
                        Name = reader["name"].ToString(),
                        UnitPrice = Convert.ToDecimal(reader["unit_price"]),
                        Type = reader["type"].ToString(),
                        Note = reader["note"].ToString()
                    };

                    dishes.Add(dish);
                }
                return dishes;
            }
            return null;
        }

        private List<Service> GetServicesByReservationId(string id)
        {
            List<Service> services = new List<Service>();
            var cmd = "SELECT * FROM services WHERE id IN (SELECT service_id FROM selected_services WHERE reservation_id=@reservation_id)";
            var idParam = new SqlParameter("@reservation_id", SqlDbType.VarChar) { Value = id };
            var reader = SqlHelper.ExecuteReader(_connectionString, CommandType.Text, cmd, idParam);

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    var service = new Service()
                    {
                        Id = reader["id"].ToString(),
                        Name = reader["name"].ToString(),
                        UnitPrice = Convert.ToDecimal(reader["unit_price"])
                    };

                    services.Add(service);
                }
                return services;
            }
            return null;
        }

        public ReservationResponse GetReservations()
        {
            try
            {
                var reservs = new List<Reservation>();
                var cmd = "SELECT reservations.*, fullname, phone FROM reservations JOIN customers ON customers.id=reservations.customer_id;";

                var reader = SqlHelper.ExecuteReader(_connectionString, CommandType.Text, cmd);

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var reserv = new Reservation()
                        {
                            Id = reader["id"].ToString(),
                            Customer = new Customer()
                            {
                                Id = reader["customer_id"].ToString(),
                                Fullname = reader["fullname"].ToString(),
                                Phone = reader["phone"].ToString()
                            },
                            ReservingDate = DateTime.Parse(reader["reserving_date"].ToString(), CultureInfo.InvariantCulture),
                            OrganizingDate = DateTime.Parse(reader["organizing_date"].ToString(), CultureInfo.InvariantCulture),
                            Hall = new Hall() { Id = reader["hall_id"].ToString() },
                            Duration = Convert.ToInt32(reader["duration"]),
                            EstimatedCost = Convert.ToDecimal((reader["estimating_cost"])),
                            NumOfTables = Convert.ToInt32(reader["num_of_tables"]),
                            Purpose = reader["purpose"].ToString(),
                            Status = reader["status"].ToString()
                        };

                        reservs.Add(reserv);
                    }
                    return new ReservationResponse(true, "Fetch data successful", reservs);
                }
                else return new ReservationResponse(false, "Fetch data failed");

                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new ReservationResponse(false, ex.Message);
            }
        }

        public ReservationResponse GetReservationById(string id)
        {
            try
            {
                var cmd = "SELECT * FROM reservations WHERE id = @id";
                var reader = SqlHelper.ExecuteReader(_connectionString, CommandType.Text, cmd, new SqlParameter("@id", id));
                if (reader.Read())
                {
                    var reserv = new Reservation()
                    {
                        Id = reader["id"].ToString(),
                        Customer = new Customer() { Id = reader["customer_id"].ToString() },
                        ReservingDate = DateTime.Parse(reader["reserving_date"].ToString(), CultureInfo.InvariantCulture),
                        OrganizingDate = DateTime.Parse(reader["organizing_date"].ToString(), CultureInfo.InvariantCulture),
                        Hall = new Hall() { Id = reader["hall_id"].ToString() },
                        Duration = Convert.ToInt32(reader["duration"]),
                        EstimatedCost = Convert.ToDecimal((reader["estimating_cost"])),
                        NumOfTables = Convert.ToInt32(reader["num_of_tables"]),
                        Purpose = reader["purpose"].ToString(),
                        Status = reader["status"].ToString()
                    };

                    reserv.Hall = _hallRepository.GetHallById(reserv.Hall.Id);
                    reserv.Customer = _customerRepository.GetCustomerById(reserv.Customer.Id);
                    reserv.Services = GetServicesByReservationId(reserv.Id);
                    reserv.Menu = GetDishesByReservationId(reserv.Id);

                    return new ReservationResponse(true, "Fetch data successfully", reserv);
                }
                return new ReservationResponse(false, "Failed to fetch data");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new ReservationResponse(false, ex.Message);
            }
        }

        public ReservationResponse CancelReservation(string id)
        {
            try
            {
                var cmd = "UPDATE reservations SET status=@status WHERE id=@id";
                var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@status", SqlDbType.NVarChar) { Value = "canceled" },
                    new SqlParameter("@id", SqlDbType.Char) { Value = id }
                };

                SqlHelper.ExecuteNonQuery(_connectionString, CommandType.Text, cmd, parameters.ToArray());
                return new ReservationResponse(true, "Update teacher successful");
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new ReservationResponse(false, ex.Message);
            }
        }

        public bool CheckValidOrganizingDatetime(DateTime dateTime)
        {
            try
            {
                var cmd = "SELECT * FROM reservations WHERE DATEDIFF(dd, organizing_date, @d)=0 AND DATEPART(hour, organizing_date)=DATEPART(hour, @d)";
                var res = SqlHelper.ExecuteReader(_connectionString, CommandType.Text, cmd,
                    new SqlParameter("@d", SqlDbType.SmallDateTime) { Value = dateTime });

                if (res.HasRows) return false;
                return true;
            } 
            catch { return false; }
        }

        private int InsertReservationServices(List<Service> services, string reserv_id)
        {
            try
            {
                var cmd = "INSERT INTO selected_services ([reservation_id], [service_id]) VALUES (@reservation_id, @service_id)";
                SqlParameter reservIdParam = new SqlParameter("@reservation_id", SqlDbType.VarChar) { Value = reserv_id };

                services.ForEach(s =>
                {
                    SqlParameter[] parameters = new SqlParameter[] {
                        new SqlParameter("@service_id", SqlDbType.VarChar) { Value = s.Id },
                        reservIdParam
                    };
                    SqlHelper.ExecuteNonQuery(_connectionString, CommandType.Text, cmd, parameters);

                });
                
                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return -1;
            }
        }

        private int InsertReservationMenu(List<Dish> dishes, string reserv_id)
        {
            try
            {
                var cmd = "INSERT INTO menus ([reservation_id], [dish_id]) VALUES (@reservation_id, @dish_id)";
                SqlParameter reservIdParam = new SqlParameter("@reservation_id", SqlDbType.VarChar) { Value = reserv_id };

                dishes.ForEach(s =>
                {
                    SqlParameter[] parameters = new SqlParameter[] {
                        new SqlParameter("@dish_id", SqlDbType.VarChar) { Value = s.Id },
                        reservIdParam
                    };
                    SqlHelper.ExecuteNonQuery(_connectionString, CommandType.Text, cmd, parameters);

                });

                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return -1;
            }
        }

        private int DeleteReservationServices(string reserv_id)
        {
            try
            {
                var cmd = "DELETE FROM selected_services WHERE reservation_id=@reserv_id";
                SqlParameter reservIdParam = new SqlParameter("@reserv_id", SqlDbType.VarChar) { Value = reserv_id };

                return SqlHelper.ExecuteNonQuery(_connectionString, CommandType.Text, cmd, reservIdParam);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return -1;
            }
        }

        private int DeleteReservationMenu(string reserv_id)
        {
            try
            {
                var cmd = "DELETE FROM menus WHERE reservation_id=@reserv_id";
                SqlParameter reservIdParam = new SqlParameter("@reserv_id", SqlDbType.VarChar) { Value = reserv_id };

                return SqlHelper.ExecuteNonQuery(_connectionString, CommandType.Text, cmd, reservIdParam);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return -1;
            }
        }

        private void UpdateReservationServices(List<Service> services, string reserv_id)
        {
            DeleteReservationServices(reserv_id);
            InsertReservationServices(services, reserv_id);
        }

        private void UpdateReservationMenu(List<Dish> dishes, string reserv_id)
        {
            DeleteReservationMenu(reserv_id);
            InsertReservationMenu(dishes, reserv_id);
        }

        public ReservationResponse InsertReservation(Reservation reservation)
        {
            try
            {
                var cmd = "INSERT INTO reservations (customer_id, reserving_date, organizing_date, hall_id, duration, num_of_tables, purpose, status) "
                    + "VALUES (@customer_id, @reserving_date, @organizing_date, @hall_id, @duration, @num_of_tables, @purpose, @status)";

                SqlParameter[] parameters = new SqlParameter[] {
                    new SqlParameter("@customer_id", SqlDbType.VarChar) { Value=reservation.Customer.Id },
                    new SqlParameter("@reserving_date", SqlDbType.SmallDateTime) { Value=DateTime.Now },
                    new SqlParameter("@organizing_date", SqlDbType.SmallDateTime) { Value=reservation.OrganizingDate },
                    new SqlParameter("@hall_id", SqlDbType.VarChar) {Value=reservation.Hall.Id},
                    new SqlParameter("@duration", SqlDbType.Int) {Value=reservation.Duration},
                    new SqlParameter("@num_of_tables", SqlDbType.Int) {Value=reservation.NumOfTables},
                    new SqlParameter("@purpose", SqlDbType.NVarChar) { Value=reservation.Purpose },
                    new SqlParameter("@status", SqlDbType.NVarChar) { Value="Pending" }
                };

                int rowAffected = SqlHelper.ExecuteNonQuery(_connectionString, CommandType.Text, cmd, parameters);

                if (rowAffected == 0)
                {
                    return new ReservationResponse(false, "Insert failed");
                }

                var cmdToGetNewReserv = "SELECT TOP 1 id FROM reservations ORDER BY id DESC";
                var res = SqlHelper.ExecuteScalar(_connectionString, CommandType.Text, cmdToGetNewReserv);
                string id = res.ToString();

                InsertReservationMenu(reservation.Menu, id);
                InsertReservationServices(reservation.Services, id);

                return new ReservationResponse(true, "Insert successfully");
            }
            catch (Exception ex)
            {
                return new ReservationResponse(false, ex.Message);
            }
        }

        public ReservationResponse UpdateReservation (Reservation reservation)
        {
            if (reservation == null) return new ReservationResponse(false, "Invalid data");
            if (string.IsNullOrEmpty(reservation.Id)) return new ReservationResponse(false, "Id can not be null");

            try
            {
                var cmd = "UPDATE reservations SET organizing_date = @organizing_date, hall_id = @hall_id, duration=@duration, num_of_tables=@num_of_tables, purpose=@purpose WHERE id = @id";

                var parameters = new SqlParameter[]
                {
                    new SqlParameter("@organizing_date", SqlDbType.SmallDateTime) { Value = reservation.OrganizingDate },
                    new SqlParameter("@hall_id", SqlDbType.VarChar) { Value = reservation.Hall.Id },
                    new SqlParameter("@duration", SqlDbType.Int) { Value = reservation.Duration },
                    new SqlParameter("@num_of_tables", SqlDbType.Int) { Value = reservation.NumOfTables },
                    new SqlParameter("@purpose", SqlDbType.Char) { Value = reservation.Purpose },
                    new SqlParameter("@id", SqlDbType.VarChar) { Value = reservation.Id }
                };

                SqlHelper.ExecuteNonQuery(_connectionString, CommandType.Text, cmd, parameters);

                UpdateReservationMenu(reservation.Menu, reservation.Id);
                UpdateReservationServices(reservation.Services, reservation.Id);

                return new ReservationResponse(true, "Update successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new ReservationResponse(false, ex.Message);
            }
        }

        public int InsertSelectedServices(string reserv_id, string service_id)
        {
            try
            {
                var cmd = "INSERT INTO selected_services(reservation_id, service_id) VALUES (@reserv_id, @service_id)";
                SqlParameter[] parameters = new SqlParameter[] {
                    new SqlParameter("@reserv_id", SqlDbType.VarChar) { Value=reserv_id },
                    new SqlParameter("@service_id", SqlDbType.VarChar) { Value=service_id },
                };
                int res = SqlHelper.ExecuteNonQuery(_connectionString, cmd, CommandType.Text, cmd, parameters);
                return res;
            } 
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return -1;
            }
        }

        public int DeleteSelectedServices(string reserv_id, string service_id)
        {
            try
            {
                var cmd = "DELETE FROM selected_services WHERE reservation_id=@reserv_id AND service_id=@service_id";
                SqlParameter[] parameters = new SqlParameter[] {
                    new SqlParameter("@reserv_id", SqlDbType.VarChar) { Value=reserv_id },
                    new SqlParameter("@service_id", SqlDbType.VarChar) { Value=service_id },
                };
                int res = SqlHelper.ExecuteNonQuery(_connectionString, cmd, CommandType.Text, cmd, parameters);
                return res;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return -1;
            }
        }

        public int InsertMenu(string reserv_id, string dish_id)
        {
            try
            {
                var cmd = "INSERT INTO menus(reservation_id, dish_id) VALUES (@reserv_id, @dish_id)";
                SqlParameter[] parameters = new SqlParameter[] {
                    new SqlParameter("@reserv_id", SqlDbType.VarChar) { Value=reserv_id },
                    new SqlParameter("@dish_id", SqlDbType.VarChar) { Value=dish_id },
                };
                int res = SqlHelper.ExecuteNonQuery(_connectionString, cmd, CommandType.Text, cmd, parameters);
                return res;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return -1;
            }
        }

        public int DeleteMenu(string reserv_id, string dish_id)
        {
            try
            {
                var cmd = "DELETE FROM menus WHERE reservation_id=@reserv_id AND dish_id=@dish_id";
                SqlParameter[] parameters = new SqlParameter[] {
                    new SqlParameter("@reserv_id", SqlDbType.VarChar) { Value=reserv_id },
                    new SqlParameter("@dish_id", SqlDbType.VarChar) { Value=dish_id },
                };
                int res = SqlHelper.ExecuteNonQuery(_connectionString, cmd, CommandType.Text, cmd, parameters);
                return res;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return -1;
            }
        }

        public decimal GetEstimateCost(string resverv_id)
        {
            var cmd = "SELECT estimating_cost FROM reservations WHERE id=@id";
            SqlParameter param = new SqlParameter("@id", SqlDbType.VarChar) { Value = resverv_id };
            var res = SqlHelper.ExecuteScalar(_connectionString, cmd, CommandType.Text, param);
            return Convert.ToDecimal(res);
        }


    }
}
