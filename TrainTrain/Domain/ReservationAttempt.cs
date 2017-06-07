using System.Collections.Generic;

namespace TrainTrain
{
    public class ReservationAttempt
    {
        public string TrainId { get; }
        private readonly int seatsRequestedCount;
        public List<Seat> AvailableSeats { get; }

        public ReservationAttempt(int seatsRequestedCount, string trainId) : this(seatsRequestedCount, new List<Seat>(), trainId)
        {
        }

        public ReservationAttempt(int seatsRequestedCount, List<Seat> availableSeats, string trainId)
        {
            this.seatsRequestedCount = seatsRequestedCount;
            this.AvailableSeats = availableSeats;
            this.TrainId = trainId;
        }

        public bool IsFullfiled { get { return AvailableSeats.Count == seatsRequestedCount; } }

        public void AssignBookingReference(string bookingRef)
        {
            this.BookingReference = bookingRef;

            foreach (var availableSeat in this.AvailableSeats)
            {
                availableSeat.BookingRef = bookingRef;
            }
        }

        public string BookingReference { get; private set; }
    }
}