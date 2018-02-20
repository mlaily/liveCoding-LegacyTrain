using System.Collections.Generic;

namespace TrainTrain.Domain
{
    public class ReservationAttempt
    {
        private readonly int _seatRequestedCount;
        public string BookingReference { get; private set; }
        public string TrainId { get; }
        public List<Seat> Seats { get; }

        public ReservationAttempt(string trainId, int seatRequestedCount, List<Seat> seats)
        {
            _seatRequestedCount = seatRequestedCount;
            TrainId = trainId;
            Seats = seats;
        }

        public bool IsFulfilled => Seats.Count == _seatRequestedCount;

        public void AssignBookingReference(string bookingReference)
        {
            BookingReference = bookingReference;
            foreach (var availableSeat in Seats)
            {
                availableSeat.BookingRef = bookingReference;
            }
        }

        public Reservation Confirm()
        {
            return new Reservation(TrainId, BookingReference, Seats);
        }
    }
}