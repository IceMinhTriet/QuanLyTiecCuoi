using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTQuanLyTiecCuoi.Entities.Responses
{
    class HallResponse : Response
    {
        public HallType HallType { get; set; }
        public Hall Hall { get; set; }
        public List<HallType> HallTypes { get; set; }
        public List<Hall> Halls { get; set; }

        public HallResponse(bool isSuccess, string message) : base(isSuccess, message)
        {
        }

        public HallResponse(bool isSuccess, string message, HallType hallType) : base(isSuccess, message)
        {
            HallType = hallType;
        }

        public HallResponse(bool isSuccess, string message, Hall hall) : base(isSuccess, message)
        {
            Hall = hall;
        }

        public HallResponse(bool isSuccess, string message, List<HallType> hallTypes) : base(isSuccess, message)
        {
            HallTypes = hallTypes;
        }

        public HallResponse(bool isSuccess, string message, List<Hall> halls) : base(isSuccess, message)
        {
            Halls = halls;
        }
    }
}
