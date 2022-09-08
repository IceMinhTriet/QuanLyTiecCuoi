using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTQuanLyTiecCuoi.Entities
{
    public class Reservation
    {
        public string Id { get; set; }
        public Customer Customer { get; set; }
        public DateTime ReservingDate { get; set; }
        public DateTime OrganizingDate { get; set; }
        public Hall Hall { get; set; }
        public int Duration { get; set; }
        public decimal EstimatedCost { get; set; }
        public int NumOfTables { get; set; }
        public string Purpose { get; set; }
        public string Status { get; set; }
        public List<Dish> Menu { get; set; }
        public List<Service> Services { get; set; }
    }
}
