using System;
using System.Collections.Generic;
using System.Linq;

namespace TrainTrain
{
    public class Train
    {
        public Dictionary<string, Coach> Coaches { get; } = new Dictionary<string, Coach>();
        public string TrainId { get; }
        public int MaxSeat => Seats.Count;
        public int ReservedSeats { get { return Seats.Count(s => !s.IsAvailable); }}
        public List<Seat> Seats { get; set; }

        public Train(string trainId, List<Seat> seats)
        {
            TrainId = trainId;
            Seats = seats;

            foreach (var seat in seats)
            {
                if (!Coaches.ContainsKey(seat.CoachName))
                {
                    Coaches[seat.CoachName] = new Coach(seat.CoachName);
                }
                Coaches[seat.CoachName] = Coaches[seat.CoachName].AddSeat(seat);
            }
        }

        public bool DoesNotExceedTrainOvervallCapityLimit(int seatsRequestedCount)
        {
            return ReservedSeats + seatsRequestedCount <= Math.Floor(ThreasholdTrainCapacity.ReservationRateLimit * MaxSeat);
        }

        public ReservationAttempt BuildReservationAttempt(int seatsRequestedCount)
        {
            foreach (var coach in Coaches.Values)
            {
                var reservationAttempt = coach.BuildReservationAttempt(TrainId, seatsRequestedCount);
                if (reservationAttempt.IsFulFilled)
                {
                    return reservationAttempt;
                }
            }
            return new ReservationAttemptFailure(TrainId,seatsRequestedCount);
        }
  
    }
}