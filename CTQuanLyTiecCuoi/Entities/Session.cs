using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTQuanLyTiecCuoi.Entities
{
    class Session
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public DateTime StartingAt { get; set; }

        public Session (string userId, string userName, DateTime startingAt)
        {
            UserId = userId;
            UserName = userName;
            StartingAt = startingAt;
        }

        public Session()
        {
            UserId = "";
            UserName = "";
        }
    }
}
