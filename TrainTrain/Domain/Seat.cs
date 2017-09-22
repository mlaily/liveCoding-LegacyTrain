namespace TrainTrain.Domain
{
    public class Seat
    {
        public string CoachName { get; }
        public int SeatNumber { get; }
        public string BookingRef { get; set;  }
        public bool IsAvailable => BookingRef == string.Empty;

        public Seat(string coachName, int seatNumber, string bookingRef)
        {
            this.CoachName = coachName;
            this.SeatNumber = seatNumber;
            this.BookingRef = bookingRef;
        }

        public bool IsReserved()
        {
            return !this.IsAvailable;
        }
    }
}