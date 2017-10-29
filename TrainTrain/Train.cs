using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace TrainTrain
{
    public class Train
    {
        public int MaxSeat => Seats.Count;
        public int ReservedSeats { get { return Seats.Count(s => s.BookingRef != string.Empty); }}
        public List<Seat> Seats { get; set; }

        public Train(List<Seat> seats)
        {
            this.Seats = seats;
        }
    }
}