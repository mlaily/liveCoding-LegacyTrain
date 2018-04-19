using System;
using System.Collections.Generic;
using System.Linq;

namespace TrainTrain.Domain
{
    public class Train
    {
        public string TrainId { get; }
        public int ReservedSeats => Seats.Count(s => !string.IsNullOrEmpty(s.BookingRef));
        public List<Seat> Seats => Coaches.SelectMany(c => c.Value.Seats).ToList();
        public Dictionary<string, Coach> Coaches { get; set; } = new Dictionary<string, Coach>();

        public Train(string trainId, List<Seat> seats)
        {
            TrainId = trainId;
            foreach (var seat in seats)
            {
                if (!Coaches.ContainsKey(seat.CoachName))
                {
                    Coaches[seat.CoachName] = new Coach(seat.CoachName);
                }

                Coaches[seat.CoachName].AddSeat(seat);
            }
        }

        public int GetMaxSeat()
        {
            return Seats.Count;
        }

        public bool DoNotExceedTrainCapacity(int seatsRequestedCount)
        {
            return (this.ReservedSeats + seatsRequestedCount) <= Math.Floor(ThreasholdManager.GetMaxRes() * this.GetMaxSeat());
        }

        public ReservationAttempt BuildReservationAttempt(int seatsRequestedCount)
        {
            foreach (var coach in Coaches.Values)
            {
                var reservationAttempt = coach.BuildReservationAttempt(TrainId, seatsRequestedCount);
                if (reservationAttempt.IsFulfilled)
                {
                    return reservationAttempt;
                }
            }

            return new ReservationAttempt(TrainId, seatsRequestedCount, new List<Seat>());

            
        }
    }
}