using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTQuanLyTiecCuoi.Entities.Responses
{
    class CustomerResponse : Response
    {
        public Customer Customer { get; set; }

        public CustomerResponse(bool isSuccess, string message) : base(isSuccess, message)
        {
        }

        public CustomerResponse(bool isSuccess, string message, Customer a) : base(isSuccess, message)
        {
            Customer = a;
        }
    }
}
