using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTQuanLyTiecCuoi.Entities.Responses
{
    class UserResponse : Response
    {
        public Account Account { get; set; }

        public UserResponse(bool isSuccess, string message) : base(isSuccess, message)
        {
        }

        public UserResponse(bool isSuccess, string message, Account a) : base(isSuccess, message)
        {
            Account = a;
        }
    }
}
