namespace TrainTrain
{
    public class Seat
    {
        public string CoachName { get; }
        public int SeatNumber { get; }
        public string BookingRef { get; set;  }
        public bool IsAvailable => this.BookingRef == string.Empty;
        public bool IsReserved => !IsAvailable;


        public Seat(string coachName, int seatNumber, string bookingRef)
        {
            this.CoachName = coachName;
            this.SeatNumber = seatNumber;
            this.BookingRef = bookingRef;
        }
    }
}