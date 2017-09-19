using System.Collections.Generic;

namespace TrainTrain
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

    public class ReservationAttempt : Reservation
    {
        protected readonly int SeatsRequestedCount;

        public ReservationAttempt(string trainId, int seatsRequestedCount, List<Seat> seats):base(trainId, string.Empty, seats)
        {
            SeatsRequestedCount = seatsRequestedCount;
        }

        public Reservation Confirm()
        {
            return new Reservation(TrainId, BookingRef, Seats);
        }

        public bool IsFulfilled => this.Seats.Count == SeatsRequestedCount;

        public void AssignBookingReference(string bookingRef)
        {
            BookingRef = bookingRef;
            foreach (var availableSeat in this.Seats)
            {
                availableSeat.BookingRef = bookingRef;
            }
        }
    }
}