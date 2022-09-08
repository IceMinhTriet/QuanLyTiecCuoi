using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTQuanLyTiecCuoi.Entities
{
    public class Dish
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public decimal UnitPrice { get; set; }
        public string Type { get; set; }
        public string Note { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            Dish objAsDish = obj as Dish;
            if (objAsDish == null) return false;
            else return Equals(objAsDish);
        }

        public bool Equals(Dish other)
        {
            if (other == null) return false;
            return (Id.Equals(other.Id));
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
