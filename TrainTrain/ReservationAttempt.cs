using System.Collections.Generic;

namespace TrainTrain
{
    public class ReservationAttempt
    {
        private readonly int _seatsRequestedCount;
        public List<Seat> Seats { get; }

        public ReservationAttempt(int seatsRequestedCount, List<Seat> seats)
        {
            _seatsRequestedCount = seatsRequestedCount;
            Seats = seats;
        }

        public bool IsFulfilled => this.Seats.Count == _seatsRequestedCount;

        public void AssignBookingReference(string bookingRef)
        {
            foreach (var availableSeat in this.Seats)
            {
                availableSeat.BookingRef = bookingRef;
            }
        }
    }
}