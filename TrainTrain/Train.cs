using System.Collections.Generic;
using System.Linq;

namespace TrainTrain
{
    public class Train
    {
        public Train(List<Seat> seats)
        {
            this.Seats = seats;
        }

        public int GetMaxSeat()
        {
            return this.Seats.Count;
        }

        public int ReservedSeats
        {
            get { return Seats.Count(s => !string.IsNullOrEmpty(s.BookingRef)); }
        }
        public List<Seat> Seats { get; set; }

        public bool HasLessThanThreshold(int i)
        {
            return ReservedSeats < i;
        }
    }

    public class TrainJsonPoco
    {
        public List<SeatJsonPoco> seats { get; set;  }

        public TrainJsonPoco()
        {
            this.seats = new List<SeatJsonPoco>();
        }
    }

    public class SeatJsonPoco
    {
        public string booking_reference { get; set; }
        public string seat_number { get; set; }
        public string coach { get; set; }
    }
}