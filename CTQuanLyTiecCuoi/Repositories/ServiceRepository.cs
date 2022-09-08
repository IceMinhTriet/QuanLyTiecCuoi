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
    class ServiceRepository
    {
        private readonly string _connectionString;

        public ServiceRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public string ConnectionString { get { return _connectionString; } }

        private List<Hall> GetHallsContainingService(string service_id)
        {
            List<Hall> halls = new List<Hall>();
            var cmd = "SELECT id, name FROM halls WHERE id IN (SELECT hall_id FROM hall_services WHERE service_id=@service_id)";
            var serviceIdParam = new SqlParameter("@service_id", SqlDbType.VarChar) { Value = service_id };
            var reader = SqlHelper.ExecuteReader(_connectionString, CommandType.Text, cmd, serviceIdParam);

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    var hall = new Hall()
                    {
                        Id = reader["id"].ToString(),
                        Name = reader["name"].ToString(),
                    };

                    halls.Add(hall);
                }
                return halls;
            }
            return null;
        }

        public ServiceResponse GetSevicesByHallId(string hall_id)
        {
            List<Service> services = new List<Service>();
            var cmd = "SELECT * FROM services WHERE id IN (SELECT service_id FROM hall_services WHERE hall_id=@hall_id)";
            var serviceIdParam = new SqlParameter("@hall_id", SqlDbType.VarChar) { Value = hall_id };
            var reader = SqlHelper.ExecuteReader(_connectionString, CommandType.Text, cmd, serviceIdParam);

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
                return new ServiceResponse(true, "Fetch data successfully", services);
            }
            return new ServiceResponse(false, "Failed to fetch data");
        }

        private int UpdateHallServices(List<string> hall_ids, string service_id)
        {
            var deleteCmd = "DELETE FROM hall_services WHERE service_id = @service_id";

            try
            {
                SqlParameter serviceIdParam = new SqlParameter("@service_id", SqlDbType.VarChar) { Value = service_id };
                SqlHelper.ExecuteNonQuery(_connectionString, CommandType.Text, deleteCmd, serviceIdParam);

                InsertHallServices(hall_ids, service_id);
                return 0;
            }
            catch
            {
                return -1;
            }

        }

        private int InsertHallServices(List<string> hall_ids, string service_id)
        {
            var insertCmd = "INSERT INTO hall_services(hall_id, service_id) VALUES(@hall_id, @service_id)";

            try
            {
                SqlParameter serviceIdParam = new SqlParameter("@service_id", SqlDbType.VarChar) { Value = service_id };
                
                hall_ids.ForEach(h =>
                {
                    SqlParameter hallIdParam = new SqlParameter("@hall_id", SqlDbType.VarChar) { Value = h };
                    SqlHelper.ExecuteNonQuery(_connectionString, CommandType.Text, insertCmd, new SqlParameter[] { serviceIdParam, hallIdParam });
                });
                return 0;
            }
            catch
            {
                return -1;
            }
        }

        private int DeleteHallServices(string service_id)
        {
            var cmd = "DELETE FROM hall_services WHERE service_id=@service_id";
            try
            {
                SqlParameter param = new SqlParameter("@service_id", SqlDbType.VarChar) { Value = service_id };
                return SqlHelper.ExecuteNonQuery(_connectionString, CommandType.Text, cmd, param);
            }
            catch
            {
                return -1;
            }
        }

        private bool IsServiceAvailableToDelete(string service_id)
        {
            var cmd = "SELECT * FROM selected_services WHERE service_id=@service_id";
            try
            {
                var reader = SqlHelper.ExecuteReader(_connectionString, CommandType.Text, cmd, new SqlParameter("@service_id", service_id));
                return !reader.HasRows;
            }
            catch
            {
                return false;
            }

        }

        public ServiceResponse GetServices()
        {
            try
            {
                var services = new List<Service>();
                var cmd = "SELECT * FROM services";

                var reader = SqlHelper.ExecuteReader(_connectionString, CommandType.Text, cmd);

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var service = new Service()
                        {
                            Id = reader["id"].ToString(),
                            Name = reader["name"].ToString(),
                            UnitPrice = Convert.ToDecimal(reader["unit_price"]),
                            Halls = new List<Hall>()
                        };

                        service.Halls = GetHallsContainingService(service.Id);

                        services.Add(service);
                    }

                }
                else return new ServiceResponse(false, "Fetch data failed");

                return new ServiceResponse(true, "Fetch data successful", services);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new ServiceResponse(false, "Fetch data failed");
            }
        }

        public ServiceResponse UpdateService(Service service)
        {
            if (service == null) return new ServiceResponse(false, "Invalid data");
            if (string.IsNullOrEmpty(service.Id)) return new ServiceResponse(false, "Id can not be null");

            try
            {
                var cmd = "UPDATE services SET [name] = @name, unit_price = @unit_price WHERE id = @id";

                var parameters = new SqlParameter[]
                {
                    new SqlParameter("@name", SqlDbType.NVarChar) { Value = service.Name },
                    new SqlParameter("@unit_price", SqlDbType.Money) { Value = service.UnitPrice },
                    new SqlParameter("@id", SqlDbType.VarChar) { Value = service.Id }
                };

                SqlHelper.ExecuteNonQuery(_connectionString, CommandType.Text, cmd, parameters);

                List<string> hall_ids = service.Halls.Select(h => h.Id).ToList();
                //service.Halls.ForEach(h => hall_ids.Add(h.Id));

                UpdateHallServices(hall_ids, service.Id);
                return new ServiceResponse(true, "Update successfully", service);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new ServiceResponse(false,ex.Message);
            }
        }

        public ServiceResponse InsertService(Service service)
        {
            try
            {
                var cmd = "INSERT INTO services ([name], [unit_price]) VALUES (@name, @unit_price)";

                SqlParameter[] parameters = new SqlParameter[] {
                    new SqlParameter("@name", SqlDbType.NVarChar) { Value = service.Name },
                    new SqlParameter("@unit_price", SqlDbType.Money) { Value = service.UnitPrice }
                };
                int rowAffected = SqlHelper.ExecuteNonQuery(_connectionString, CommandType.Text, cmd, parameters);

                if (rowAffected == 0)
                {
                    return new ServiceResponse(false, "Insert failed");
                }
                var getIdCmd = "SELECT TOP 1 id FROM services ORDER BY id DESC";
                var res = SqlHelper.ExecuteScalar(_connectionString, CommandType.Text, getIdCmd);                
                service.Id = res.ToString();

                List<string> hall_ids = new List<string>();
                service.Halls.ForEach(h => hall_ids.Add(h.Id));

                InsertHallServices(hall_ids, service.Id);

                return new ServiceResponse(true, "Insert successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new ServiceResponse(false, ex.Message);
            }

        }

        public ServiceResponse DeleteService(string id)
        {
            try
            {
                if (!IsServiceAvailableToDelete(id))
                    return new ServiceResponse(false, "Service has been selected by some customers");
                

                if (DeleteHallServices(id) < 0) 
                    return new ServiceResponse(false, "Some errors occurred");

                var cmd = "DELETE FROM services WHERE id = @id";
                int rowAffected = SqlHelper.ExecuteNonQuery(_connectionString, CommandType.Text, cmd, new SqlParameter("@id", id));
                if (rowAffected == 0)
                {
                    return new ServiceResponse(true, "Delete failed");
                }

                return new ServiceResponse(true, "Delete successful");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new ServiceResponse(false, ex.Message);
            }
        }

    }
}
