using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace TrainTrain
{
    public class Train
    {
        public int MaxSeat => this.Seats.Count;
        public int ReservedSeats { get { return Seats.Count(s => s.BookingRef != string.Empty); } }
        public List<Seat> Seats { get; set; }

        public Train(List<Seat> seats)
        {
            this.Seats = seats;
        }

        public bool DoesNotExceedTrainCapacityLimit(int seatsRequestedCount)
        {
            return this.ReservedSeats + seatsRequestedCount <= Math.Floor(CapacityThreasholds.TrainMaxCapacity * this.MaxSeat);
        }

        public ReservationAttempt BuildReservationAttempt(int seatsRequestedCount)
        {
            List<Seat> availableSeats = new List<Seat>();
            // find seats to reserve
            for (int index = 0, i = 0; index < this.Seats.Count; index++)
            {
                var each = this.Seats[index];
                if (each.BookingRef == "")
                {
                    i++;
                    if (i <= seatsRequestedCount)
                    {
                        availableSeats.Add(each);
                    }
                }
            }
            return new ReservationAttempt(seatsRequestedCount, availableSeats);
        }
    }

    public class ReservationAttempt
    {
        private readonly int _seatsRequestedCount;
        public List<Seat> Seats { get; }

        public ReservationAttempt(int seatsRequestedCount, List<Seat> seats)
        {
            _seatsRequestedCount = seatsRequestedCount;
            Seats = seats;
        }

        public bool IsFulfilled => this.Seats.Count == _seatsRequestedCount;
    }
}