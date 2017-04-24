using System;
using System.Collections.Generic;

namespace TrainTrain
{
    public class Train
    {
        public Train(string trainTopol)
        {
            throw new NotImplementedException();
        }

        public int MaxSeat { get; set; }

        public int ReservedSeats { get; set; }
        public List<Seat> Seats { get; set; }

        public bool HasLessThanThreshold(int i)
        {
            throw new NotImplementedException();
        }
    }
}