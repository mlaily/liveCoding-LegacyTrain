using System.Collections.Generic;

namespace TrainTrain
{
    public class FailedReservation : Reservation
    {
        public FailedReservation(string trainId):base(trainId, string.Empty, new List<Seat>())
        {
            
        }

        public FailedReservation(string trainId, string bookingRef, List<Seat> seats) : base(trainId, bookingRef, seats)
        {
        }
    }
}