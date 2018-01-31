using System.Collections.Generic;
using System.Linq;

namespace TrainTrain
{
    public class Coach
    {
        public string CoachName { get; }
        public IReadOnlyCollection<Seat> Seats { get; }

        public Coach(string coachName) : this(coachName, new List<Seat>())
        {
        }

        public Coach(string coachName, IReadOnlyCollection<Seat> seats)
        {
            CoachName = coachName;
            Seats = seats;
        }

        public Coach AddSeat(Seat seat)
        {
            return new Coach(seat.CoachName, new List<Seat>(Seats) { seat });
        }

        public ReservationAttempt BuildReservationAttempt(string trainId, int seatsRequestedCount)
        {
            var availableSeats =
                Seats.Where(s => s.IsAvailable).Take(seatsRequestedCount).ToList();
          
            return new ReservationAttempt(trainId, seatsRequestedCount, availableSeats);
        }
    }
}