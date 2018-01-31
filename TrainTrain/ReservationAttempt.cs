using System.Collections.Generic;
using System.Linq;

namespace TrainTrain
{
    public class ReservationAttempt
    {
        private readonly int _seatsRequestedCount;

        public string TrainId { get; }
        public string BookingReference { get; private set; }
        public List<Seat> Seats { get; private set; }

        public bool IsFulFilled => Seats.Count == _seatsRequestedCount;

        public ReservationAttempt(string trainId, int seatsRequestedCount, List<Seat> seats)
        {
            _seatsRequestedCount = seatsRequestedCount;
            TrainId = trainId;
            Seats = seats;
        }

        public void AssignBookingReference(string bookingReference)
        {
            BookingReference = bookingReference;
            Seats = Seats.Select(seat => new Seat(seat.CoachName, seat.SeatNumber, bookingReference)).ToList();
        }

        public Reservation Confirm()
        {
            return new Reservation(TrainId, BookingReference, Seats);
        }
    }
}