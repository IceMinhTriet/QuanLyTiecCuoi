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
    class HallRepository
    {
        private readonly string _connectionString;
        private Session _session;

        public HallRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public string ConnectionString { get { return _connectionString; } }
        public Session Session { get { return _session; } }

        ///////  Methods to manipulate hall types ////////
        ///
        public HallResponse GetHallTypes()
        {
            try
            {
                var types = new List<HallType>();
                var cmd = "SELECT * FROM hall_types";

                var reader = SqlHelper.ExecuteReader(_connectionString, CommandType.Text, cmd);

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var type = new HallType()
                        {
                            Id = reader["id"].ToString(),
                            TypeName = reader["name"].ToString(),
                            UnitPrice = Convert.ToDecimal(reader["unit_price"])
                        };

                        types.Add(type);
                    }

                }
                else return new HallResponse(false, "Fetch data failed");

                return new HallResponse(true, "Fetch data successful", types);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new HallResponse(false, "Fetch data failed");
            }
        }

        public HallResponse GetHallTypeById(string typeId)
        {
            try
            {
                var cmd = "SELECT * FROM hall_types WHERE id = @id";
                var reader = SqlHelper.ExecuteReader(_connectionString, CommandType.Text, cmd, new SqlParameter("@id", typeId));
                if (reader.Read())
                {
                    return new HallResponse(true, "Fetch data successfully", new HallType()
                    {
                        Id = reader["id"].ToString(),
                        TypeName = reader["name"].ToString(),
                        UnitPrice = Convert.ToDecimal(reader["unit_price"])
                    });
                }
                return new HallResponse(false, "Failed to fetch data");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new HallResponse(false, "An error occurred");
            }
        }

        public HallResponse UpdateHallType(HallType hallType)
        {
            if (hallType == null) return new HallResponse(false, "Invalid data");
            if (string.IsNullOrEmpty(hallType.Id)) return new HallResponse(false, "Id does not exist");

            try
            {
                var cmd = "UPDATE hall_types SET [name] = @name, unit_price = @unit_price WHERE id = @id";

                var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@name", SqlDbType.NVarChar) { Value = hallType.TypeName },
                    new SqlParameter("@unit_price", SqlDbType.Money) { Value = hallType.UnitPrice },
                    new SqlParameter("@id", SqlDbType.VarChar) { Value = hallType.Id }
                };

                SqlHelper.ExecuteNonQuery(_connectionString, CommandType.Text, cmd, parameters.ToArray());
                return new HallResponse(true, "Update successfully", hallType);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new HallResponse(false, "Update failed");
            }
        }

        public HallResponse InsertHallType(HallType hallType)
        {
            try
            {
                var cmd = "INSERT INTO hall_types ([name], [unit_price]) VALUES (@name, @unit_price)";

                SqlParameter[] parameters = new SqlParameter[] {
                    new SqlParameter("@name", SqlDbType.NVarChar) { Value = hallType.TypeName },
                    new SqlParameter("@unit_price", SqlDbType.Money) { Value = hallType.UnitPrice }
                };
                int rowAffected = SqlHelper.ExecuteNonQuery(_connectionString, CommandType.Text, cmd, parameters);

                if (rowAffected == 0)
                {
                    return new HallResponse(false, "Insert failed");
                }

                return new HallResponse(true, "Insert successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new HallResponse(false, "Insert failed");
            }

        }

        public HallResponse DeleteHallType(string id)
        {
            try
            {
                var cmd = "DELETE FROM hall_types WHERE id = @id";
                int rowAffected = SqlHelper.ExecuteNonQuery(_connectionString, CommandType.Text, cmd, new SqlParameter("@id", id));
                if (rowAffected == 0)
                {
                    return new HallResponse(true, "Delete failed");
                }

                return new HallResponse(true, "Delete successful");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new HallResponse(false, "Delete failed");
            }
        }

        ///////  Finnish methods of manipulating hall types ////////


        ///////  Methods to manipulate halls ////////
        ///
        public HallResponse GetHalls()
        {
            try
            {
                var halls = new List<Hall>();
                var cmd = "SELECT * FROM halls";

                var reader = SqlHelper.ExecuteReader(_connectionString, CommandType.Text, cmd);

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var hall = new Hall()
                        {
                            Id = reader["id"].ToString(),
                            Name = reader["name"].ToString(),
                            Type = new HallType(),
                            Capacity = Convert.ToInt32(reader["num_of_tables"]),
                            Address = reader["address"].ToString(),
                            Note = reader["note"].ToString()
                        };

                        string type_id = reader["type"].ToString();
                        var res = GetHallTypeById(type_id);
                        if (res.isSuccess) hall.Type = res.HallType;

                        halls.Add(hall);
                    }

                }
                else return new HallResponse(false, "Fetch data failed");

                return new HallResponse(true, "Fetch data successful", halls);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new HallResponse(false, "Fetch data failed");
            }
        }

        protected internal Hall GetHallById(string id)
        {
            try
            {
                var cmd = "SELECT * FROM halls WHERE id = @id";
                var reader = SqlHelper.ExecuteReader(_connectionString, CommandType.Text, cmd, new SqlParameter("@id", id));
                if (reader.Read())
                {
                    Hall hall = new Hall()
                    {
                        Id = reader["id"].ToString(),
                        Name = reader["name"].ToString(),
                        Capacity = Convert.ToInt32(reader["num_of_tables"]),
                        Address = reader["address"].ToString(),
                        Note = reader["note"].ToString()
                    };

                    string type_id = reader["type"].ToString();
                    var res = GetHallTypeById(type_id);
                    if (!res.isSuccess) return null;
                    hall.Type = res.HallType;

                    return hall;
                }
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public HallResponse InsertHall(Hall hall)
        {
            try
            {
                var cmd = "INSERT INTO halls ([type], [name], [num_of_tables], [address], [note]) VALUES (@type, @name, @num_of_tables, @address, @note)";

                SqlParameter[] parameters = new SqlParameter[] {
                    new SqlParameter("@name", SqlDbType.NVarChar) { Value = hall.Name},
                    new SqlParameter("@type", SqlDbType.VarChar) { Value = hall.Type.Id},
                    new SqlParameter("@num_of_tables", SqlDbType.Int) { Value = hall.Capacity},
                    new SqlParameter("@address", SqlDbType.NVarChar) { Value = hall.Address},
                    new SqlParameter("@note", SqlDbType.NVarChar) { Value = hall.Note}
                };
                int rowAffected = SqlHelper.ExecuteNonQuery(_connectionString, CommandType.Text, cmd, parameters);

                if (rowAffected == 0)
                {
                    return new HallResponse(false, "Insert failed");
                }

                return new HallResponse(true, "Insert successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new HallResponse(false, "Insert failed");
            }

        }

        public HallResponse UpdateHall(Hall hall)
        {
            if (hall == null) return new HallResponse(false, "Invalid data");
            if (string.IsNullOrEmpty(hall.Id)) return new HallResponse(false, "Id does not exist");

            try
            {
                var cmd = "UPDATE halls SET [name]=@name, [type]=@type, num_of_tables=@num_of_tables, address=@address, note=@note WHERE id = @id";

                var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@name", SqlDbType.NVarChar) { Value = hall.Name },
                    new SqlParameter("@type", SqlDbType.VarChar) { Value = hall.Type.Id },
                    new SqlParameter("@num_of_tables", SqlDbType.Int) { Value = hall.Capacity },
                    new SqlParameter("@address", SqlDbType.NVarChar) { Value = hall.Address },
                    new SqlParameter("@note", SqlDbType.NVarChar) { Value = hall.Note },
                    new SqlParameter("@id", SqlDbType.VarChar) { Value = hall.Id }
                };

                SqlHelper.ExecuteNonQuery(_connectionString, CommandType.Text, cmd, parameters.ToArray());
                return new HallResponse(true, "Update successfully", hall);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new HallResponse(false, "Update failed");
            }
        }

        public HallResponse DeleteHall(string id)
        {
            try
            {
                var cmd = "DELETE FROM halls WHERE id = @id";
                int rowAffected = SqlHelper.ExecuteNonQuery(_connectionString, CommandType.Text, cmd, new SqlParameter("@id", id));
                if (rowAffected == 0)
                {
                    return new HallResponse(true, "Delete failed");
                }

                return new HallResponse(true, "Delete successful");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new HallResponse(false, "Delete failed");
            }

        }
    }
}
