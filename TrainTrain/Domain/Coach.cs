using System.Collections.Generic;

namespace TrainTrain.Domain
{
    public class Coach
    {
        public string CoachName { get; }

        public Coach(string coachName)
        {
            CoachName = coachName;
        }

        public List<Seat> Seats { get; } = new List<Seat>();

        public void AddSeat(Seat seat)
        {
            this.Seats.Add(seat);
        }

        public ReservationAttempt BuildReservationAttempt(string trainId, int seatsRequestedCount)
        {
            var availableSeats = new List<Seat>();

            // find seats to reserve
            for (int index = 0, i = 0; index < this.Seats.Count; index++)
            {
                var seat = this.Seats[index];
                if (seat.IsAvailable())
                {
                    i++;
                    if (i <= seatsRequestedCount)
                    {
                        availableSeats.Add(seat);
                    }
                }
            }

            return new ReservationAttempt(trainId, seatsRequestedCount, availableSeats);
        }
    }
}