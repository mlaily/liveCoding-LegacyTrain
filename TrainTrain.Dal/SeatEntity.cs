using System.ComponentModel.DataAnnotations;

namespace TrainTrain.Dal
{
    public class SeatEntity
    {
        public string CoachName { get; set; }
        public int SeatNumber { get; set; }
        public string BookingRef { get; set; }
    }
}