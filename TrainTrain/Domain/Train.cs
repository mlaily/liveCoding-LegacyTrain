using System;
using System.Collections.Generic;
using System.Linq;
using Value;
using Value.Shared;

namespace TrainTrain.Domain
{
    public class Train :ValueType<Train>
    {
        public Dictionary<string, Coach> Coaches = new Dictionary<string, Coach>();

        public Train(List<Seat> seats)
        {
            foreach (var seat in seats)
            {
                if (!Coaches.ContainsKey(seat.CoachName))
                {
                    var coach = new Coach(seat.CoachName);
                    Coaches.Add(coach.CoachName, coach);
                }

                var oldCoach = Coaches[seat.CoachName];
                var newCoach = oldCoach.AddSeat(seat);
                Coaches[newCoach.CoachName] = newCoach;
            }
        }

        public int GetMaxSeat()
        {
            return Seats.Count;
        }

        public int ReservedSeats
        {
            get { return Seats.Count(s => !s.IsAvailable); }
        }

        public List<Seat> Seats => Coaches.Values.SelectMany(c => c.Seats).ToList();
        
        public bool DoesNotExceedOverallTrainCapacityThreashold(int seats)
        {
            return (ReservedSeats + seats) <= Math.Floor(ThreasholdTrainCapacity.MaxReservation * GetMaxSeat());
        }

        public ReservationAttempt BuildReservationAttempt(string traindId, int seatRequestedCount)
        {
            var availableSeats = Seats.Where(s => s.IsAvailable).Take(seatRequestedCount).ToList();
            return new ReservationAttempt(traindId, seatRequestedCount, availableSeats);
        }

        protected override IEnumerable<object> GetAllAttributesToBeUsedForEquality()
        {
            return new List<object> { new DictionaryByValue<string, Coach>(Coaches) , ReservedSeats };
        }
    }
}