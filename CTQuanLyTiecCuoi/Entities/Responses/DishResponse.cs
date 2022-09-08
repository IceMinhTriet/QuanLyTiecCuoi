using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTQuanLyTiecCuoi.Entities.Responses
{
    class DishResponse : Response
    {
        public Dish Dish { get; set; }
        public List<Dish> Dishes { get; set; }

        public DishResponse(bool isSuccess, string message) : base(isSuccess, message)
        {
        }

        public DishResponse(bool isSuccess, string message, Dish dish) : base(isSuccess, message)
        {
            Dish = dish;
        }

        public DishResponse(bool isSuccess, string message, List<Dish> dishes) : base(isSuccess, message)
        {
            Dishes = dishes;
        }
    }
}
