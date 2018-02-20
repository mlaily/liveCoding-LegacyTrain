using System.Collections.Generic;
using Value;

namespace TrainTrain.Domain
{
    public class Coach : ValueType<Coach>
    {
        public List<Seat> Seats { get; }

        public string CoachName { get; }

        public Coach(string coachName) : this(coachName, new List<Seat>())
        {
        }

        public Coach(string coachName, List<Seat> seats)
        {
            CoachName = coachName;
            Seats = seats;
        }

        public Coach AddSeat(Seat seat)
        {
            var newListOfSeats = new List<Seat>(Seats) { seat };
            return new Coach(CoachName, newListOfSeats);
        }

        protected override IEnumerable<object> GetAllAttributesToBeUsedForEquality()
        {
            return new List<object> { CoachName, new ListByValue<Seat>(Seats) };
        }
    }
}