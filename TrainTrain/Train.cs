using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace TrainTrain
{
    public class Train
    {
        public Dictionary<string, Coach> Coaches { get; } = new Dictionary<string, Coach>();
        public int MaxSeat => this.Seats.Count;
        public int ReservedSeats { get { return Seats.Count(s => s.BookingRef != string.Empty); } }
        public List<Seat> Seats { get { return Coaches.Values.SelectMany(c => c.Seats).ToList(); }}

        public Train(IEnumerable<Seat> seats)
        {
            foreach (var seat in seats)
            {
                if (!Coaches.ContainsKey(seat.CoachName))
                {
                    Coaches[seat.CoachName] = new Coach(seat.CoachName);
                }
                Coaches[seat.CoachName].AddSeat(seat);
            }
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

    public class Coach
    {
        public List<Seat> Seats { get; } = new List<Seat>();
        public string CoachName { get; }

        public Coach(string coachName)
        {
            CoachName = coachName;
        }

        public void AddSeat(Seat seat)
        {
            Seats.Add(seat);
        }
    }
}