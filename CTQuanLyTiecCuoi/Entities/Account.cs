using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTQuanLyTiecCuoi.Entities
{
    class Account
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string HashPassword { get; set; }
        public string Password { get; set; }
    }
}
