using System.Collections.Generic;
using System.Linq;

namespace TrainTrain.Domain
{
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

        public ReservationAttempt BuildReservationAttempt(string trainId, int seatsRequestedCount)
        {
            var availableSeats = Seats.Where(s => s.IsAvailable).Take(seatsRequestedCount).ToList();
            
            return new ReservationAttempt(trainId, seatsRequestedCount, availableSeats);
        }
    }
}