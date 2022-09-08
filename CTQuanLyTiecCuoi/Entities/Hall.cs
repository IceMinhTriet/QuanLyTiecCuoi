using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTQuanLyTiecCuoi.Entities
{
    public class Hall
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public HallType Type { get; set; }
        public int Capacity { get; set; }
        public string Address { get; set; }
        public string Note { get; set; }

    }

    public class HallType
    {
        public string Id { get; set; }
        public string TypeName { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
