using System.Collections.Generic;

namespace TrainTrain
{
    public class ReservationAttempt
    {
        private readonly int _seatsRequestedCount;
        public string BookingRef { get; private set; }
        public string TrainId { get; }
        public List<Seat> Seats { get; }
        public bool IsFulfilled => Seats.Count == _seatsRequestedCount;

        public ReservationAttempt(string trainId, int seatsRequestedCount, List<Seat> seats)
        {
            _seatsRequestedCount = seatsRequestedCount;
            TrainId = trainId;
            Seats = seats;
        }

        public void AssignBookingReference(string bookingRef)
        {
            BookingRef = bookingRef;
            foreach (var availableSeat in Seats)
            {
                availableSeat.BookingRef = bookingRef;
            }
        }

        public Reservation Confirm()
        {
            return new Reservation(TrainId, BookingRef, Seats);
        }
    }
}