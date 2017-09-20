using System.Collections.Generic;

namespace TrainTrain.Domain
{
    public class Reservation
    {
        public string BookingRef { get; protected set; }
        public string TrainId { get; }
        public List<Seat> Seats { get; }

        public Reservation(string trainId, string bookingRef, List<Seat> seats)
        {
            TrainId = trainId;
            Seats = seats;
            BookingRef = bookingRef;
        }
    }
}