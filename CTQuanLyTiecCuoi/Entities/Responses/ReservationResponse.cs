using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTQuanLyTiecCuoi.Entities.Responses
{
    class ReservationResponse : Response
    {
        public Reservation Reservation { get; set; }
        public List<Reservation> Reservations { get; set; }

        public ReservationResponse(bool isSuccess, string message) : base(isSuccess, message)
        {
        }

        public ReservationResponse(bool isSuccess, string message, Reservation reservation) : base(isSuccess, message)
        {
            Reservation = reservation;
        }

        public ReservationResponse(bool isSuccess, string message, List<Reservation> reservations) : base(isSuccess, message)
        {
            Reservations = reservations;
        }
    }
}
