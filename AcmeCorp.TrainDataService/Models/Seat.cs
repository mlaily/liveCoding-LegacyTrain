using System.Text;

namespace AcmeCorp.TrainDataService.Models
{
    public class Seat
    {
        public string booking_reference { get; set; }
        public string seat_number { get; set; }
        public string coach { get; set; }

        public Seat(string coach, string seatNumber, string bookingReference)
        {
            booking_reference = bookingReference;
            seat_number = seatNumber;
            this.coach = coach;
        }

        public override string ToString()
        {
            return $"\"{seat_number}{coach}\": {{\"booking_reference\": \"{booking_reference}\", \"seat_number\": \"{seat_number}\", \"coach\": \"{coach}\"}}";
        }
    }
}