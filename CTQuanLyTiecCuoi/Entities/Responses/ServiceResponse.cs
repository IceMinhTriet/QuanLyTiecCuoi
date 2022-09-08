using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTQuanLyTiecCuoi.Entities.Responses
{
    class ServiceResponse : Response
    {
        public Service Service { get; set; }
        public List<Service> Services { get; set; }

        public ServiceResponse(bool isSuccess, string message) : base(isSuccess, message)
        {
        }

        public ServiceResponse(bool isSuccess, string message, Service service ) : base(isSuccess, message)
        {
            Service = service;
        }

        public ServiceResponse(bool isSuccess, string message, List<Service> services) : base(isSuccess, message)
        {
            Services = services;
        }
    }
}
