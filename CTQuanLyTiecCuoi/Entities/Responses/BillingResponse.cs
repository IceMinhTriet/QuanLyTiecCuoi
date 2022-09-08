using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTQuanLyTiecCuoi.Entities.Responses
{
    class BillingResponse : Response
    {
        public Billing Bill { get; set; }

        public BillingResponse(bool isSuccess, string message) : base(isSuccess, message)
        {
        }

        public BillingResponse(bool isSuccess, string message, Billing reservation) : base(isSuccess, message)
        {
            Bill = reservation;
        }

    }
}
