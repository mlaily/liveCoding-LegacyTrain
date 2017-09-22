using System;
using System.Collections.Generic;
using System.Linq;

namespace TrainTrain.Domain
{
    public class Train
    {
        public Dictionary<string, Coach> Coaches { get; } = new Dictionary<string, Coach>();
        public int MaxSeats => Seats.Count;
        public int ReservedSeats { get { return Seats.Count(s => s.IsReserved()); } }

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

        public bool DoesNotExceedOveralTrainCapacity(int seatsRequestedCount)
        {
            return ReservedSeats + seatsRequestedCount <= Math.Floor(ThreasholdManager.GetMaxRes() * MaxSeats);
        }

        public ReservationAttempt BuildReservationAttempt(string trainId, int seatsRequestedCount)
        {
            foreach (var coach in Coaches.Values)
            {
                ReservationAttempt reservationAttempt = coach.BuildReservationAttempt(trainId, seatsRequestedCount);
                if (reservationAttempt.IsFulfilled)
                {
                    return reservationAttempt;
                }
            }
            return new FailedReservationAttempt(trainId,seatsRequestedCount);
        }
    }
}