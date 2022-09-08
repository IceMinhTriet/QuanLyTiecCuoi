using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTQuanLyTiecCuoi.Entities.Responses
{
    class Response
    {
        public bool isSuccess;
        public string message;

        public Response(bool isSuccess, string message)
        {
            this.isSuccess = isSuccess;
            this.message = message;
        }

    }
}
