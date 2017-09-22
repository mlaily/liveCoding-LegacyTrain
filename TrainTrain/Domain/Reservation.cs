using System.Collections.Generic;

namespace TrainTrain
{
    public class Reservation
    {
        public string TrainId { get; }
        public string BookingRef { get; }
        public List<Seat> Seats { get; }

        public Reservation(string trainId, string bookingRef, List<Seat> seats)
        {
            TrainId = trainId;
            BookingRef = bookingRef;
            Seats = seats;
        }
    }
}