namespace TrainTrain
{
    public class Seat
    {
        public string CoachName { get; }
        public int SeatNumber { get; }
        public string BookingRef { get;  }
        public bool IsAvailable => string.IsNullOrEmpty(BookingRef);


        public Seat(string coachName, int seatNumber, string bookingRef)
        {
            this.CoachName = coachName;
            this.SeatNumber = seatNumber;
            this.BookingRef = bookingRef;
        }
    }
}