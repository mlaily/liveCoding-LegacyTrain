using System.Collections.Generic;

namespace TrainTrain.Domain
{
    public class ReservationAttempt
    {
        public string TrainId { get; }
        public string BookingReference { get; set; }
        public List<Seat> Seats { get; }
        private readonly int _seatsRequestedCount;


        public ReservationAttempt(string trainId, int seatsRequestedCount, List<Seat> seats)
        {
            _seatsRequestedCount = seatsRequestedCount;
            TrainId = trainId;
            Seats = seats;
        }

        public bool IsFulfilled => this.Seats.Count == _seatsRequestedCount;


        public void AssignBookingReference(string bookingRef)
        {
            BookingReference = bookingRef;
            foreach (var availableSeat in this.Seats)
            {
                availableSeat.BookingRef = bookingRef;
            }
        }

        public Reservation Confirm()
        {
            return new Reservation(TrainId, BookingReference, Seats);
        }
    }
}