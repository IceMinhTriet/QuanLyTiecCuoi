using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTQuanLyTiecCuoi.Entities
{
    class Billing
    {
        public string Id { get; set; }
        public Reservation Reservation { get; set; }
        public decimal HallCost { get; set; }
        public decimal ServiceCost { get; set; }
        public decimal FoodCost { get; set; }
        public decimal TotalCost { get; set; }
        public decimal Deposit { get; set; }
        public decimal RemainingCost { get; set; }
        public string Note { get; set; }
        public bool IsDone { get; set; }
        public DateTime PaymentDate { get; set; }
        public string StaffId { get; set; }
    }
}
