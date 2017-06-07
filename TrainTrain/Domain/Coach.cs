using System;
using System.Collections.Generic;
using System.Linq;
using Value;

namespace TrainTrain
{
    public class Coach : ValueType<Coach>
    {
        private readonly string coachName;
        private const double CoachMaxThresholdReservableSeats = 0.70;

        public Coach(string coachName, List<Seat> seats)
        {
            this.coachName = coachName;
            this.Seats = seats;
        }

        public Coach(string coachName) : this(coachName, new List<Seat>())
        {
        }

        public Coach AddSeat(Seat seat)
        {
            var newList = new List<Seat>(this.Seats) { seat };

            return new Coach(this.coachName, newList);
        }

        public List<Seat> Seats { get; private set; } = new List<Seat>();

        public ReservationAttempt BuildReservationAttempt(int seatsRequestedCount, string trainId)
        {
            var availableSeats = new List<Seat>();
            
            // find seats to reserve
            for (int index = 0, i = 0; index < this.Seats.Count; index++)
            {
                var seat = this.Seats[index];
                if (seat.IsAvailable)
                {
                    i++;
                    if (i <= seatsRequestedCount)
                    {
                        availableSeats.Add(seat);
                    }
                }
            }

            return new ReservationAttempt(seatsRequestedCount, availableSeats, trainId);
        }

        public bool DoesNotExceedOverallTrainCapacityLimit(int seatsRequestedCount)
        {
            return this.ReservedSeats + seatsRequestedCount <= Math.Floor(CoachMaxThresholdReservableSeats * this.Seats.Count);
        }

        public int ReservedSeats => this.Seats.Count(s => ! s.IsAvailable);
        protected override IEnumerable<object> GetAllAttributesToBeUsedForEquality()
        {
            return new List<object>() {new ListByValue<Seat>(this.Seats), this.ReservedSeats, this.coachName};
        }
    }
}