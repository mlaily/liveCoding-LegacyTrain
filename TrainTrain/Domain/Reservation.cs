using System.Collections.Generic;

namespace TrainTrain.Domain
{
    public class Reservation
    {
        public string TrainId { get; set; }
        public string BookingReference { get; set; }
        public List<Seat> Seats { get; set; }

        public Reservation(string trainId, string bookingReference, List<Seat> seats)
        {
            TrainId = trainId;
            BookingReference = bookingReference;
            Seats = seats;
        }
    }
}