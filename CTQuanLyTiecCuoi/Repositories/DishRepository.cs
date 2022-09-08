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
    class DishRepository
    {
        private readonly string _connectionString;

        public DishRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public string ConnectionString { get { return _connectionString; } }

        public DishResponse GetDishes()
        {
            try
            {
                var dishes = new List<Dish>();
                var cmd = "SELECT * FROM dishes";

                var reader = SqlHelper.ExecuteReader(_connectionString, CommandType.Text, cmd);

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

                }
                else return new DishResponse(false, "Fetch data failed");

                return new DishResponse(true, "Fetch data successful", dishes);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new DishResponse(false, "Fetch data failed");
            }
        }

        public List<string> GetDishTypes()
        {
            var types = new List<string>();
            var cmd = "SELECT DISTINCT [type] FROM dishes";

            var reader = SqlHelper.ExecuteReader(_connectionString, CommandType.Text, cmd);

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    types.Add(reader["type"].ToString());
                }
                return types;
            }
            return null;
        }

        public DishResponse GetDishesByType(string type)
        {
            try
            {
                var dishes = new List<Dish>();
                var cmd = "SELECT * FROM dishes WHERE LOWER([type])=LOWER(@type)";

                var reader = SqlHelper.ExecuteReader(_connectionString, CommandType.Text, cmd, 
                    new SqlParameter("@type", SqlDbType.NVarChar) { Value = type });

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

                }
                else return new DishResponse(false, "Fetch data failed");

                return new DishResponse(true, "Fetch data successful", dishes);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new DishResponse(false, "Fetch data failed");
            }
        }

        public DishResponse UpdateDish(Dish dish)
        {
            if (dish == null) return new DishResponse(false, "Invalid data");
            if (string.IsNullOrEmpty(dish.Id)) return new DishResponse(false, "Id can not be null");

            try
            {
                var cmd = "UPDATE dishes SET [name] = @name, unit_price = @unit_price, [type]=@type, note=@note WHERE id = @id";

                var parameters = new SqlParameter[]
                {
                    new SqlParameter("@name", SqlDbType.NVarChar) { Value = dish.Name },
                    new SqlParameter("@unit_price", SqlDbType.Money) { Value = dish.UnitPrice },
                    new SqlParameter("@type", SqlDbType.NVarChar) { Value = dish.Type },
                    new SqlParameter("@note", SqlDbType.NVarChar) { Value = dish.Note },
                    new SqlParameter("@id", SqlDbType.VarChar) { Value = dish.Id }
                };

                SqlHelper.ExecuteNonQuery(_connectionString, CommandType.Text, cmd, parameters);
                
                return new DishResponse(true, "Update successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new DishResponse(false, "Update failed");
            }
        }

        public DishResponse InsertDish(Dish dish)
        {
            try
            {                
                var cmd = "INSERT INTO dishes ([name], [unit_price], [type], note) VALUES (@name, @unit_price, @type,@note)";

                SqlParameter[] parameters = new SqlParameter[] {
                    new SqlParameter("@name", SqlDbType.NVarChar) { Value = dish.Name },
                    new SqlParameter("@unit_price", SqlDbType.Money) { Value = dish.UnitPrice },
                    new SqlParameter("@type", SqlDbType.NVarChar) { Value = dish.Type },
                    new SqlParameter("@note", SqlDbType.NVarChar) { Value = dish.Note }
                };
                int rowAffected = SqlHelper.ExecuteNonQuery(_connectionString, CommandType.Text, cmd, parameters);

                if (rowAffected == 0)
                {
                    return new DishResponse(false, "Insert failed");
                }

                return new DishResponse(true, "Insert successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new DishResponse(false, "Insert failed");
            }

        }

        public DishResponse DeleteDish(string id)
        {
            try
            {
                var cmd = "DELETE FROM dishes WHERE id = @id";
                int rowAffected = SqlHelper.ExecuteNonQuery(_connectionString, CommandType.Text, cmd, new SqlParameter("@id", id));
                if (rowAffected == 0)
                {
                    return new DishResponse(true, "Delete failed");
                }

                return new DishResponse(true, "Delete successful");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new DishResponse(false, "Delete failed");
            }
        }

    }
}
