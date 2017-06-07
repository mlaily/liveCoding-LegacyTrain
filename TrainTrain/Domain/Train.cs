using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Value;
using Value.Shared;

namespace TrainTrain
{
    public class Train : ValueType<Train>
    {
        private readonly string trainId;
        private const double TrainMaxThresholdReservableSeats = 0.70;
        public Dictionary<string, Coach> Coaches = new Dictionary<string, Coach>();

        public Train(string trainTopol, string trainId)
        {
            this.trainId = trainId;
            //var sample =
            //"{\"seats\": {\"1A\": {\"booking_reference\": \"\", \"seat_number\": \"1\", \"coach\": \"A\"}, \"2A\": {\"booking_reference\": \"\", \"seat_number\": \"2\", \"coach\": \"A\"}}}";

            // Forced to workaround with dynamic parsing since the received JSON is invalid format ;-(
            dynamic parsed = JsonConvert.DeserializeObject(trainTopol);

            foreach (var token in ((Newtonsoft.Json.Linq.JContainer)parsed))
            {
                var allStuffs = ((Newtonsoft.Json.Linq.JObject) ((Newtonsoft.Json.Linq.JContainer) token).First);

                foreach (var stuff in allStuffs)
                {
                    var seatJsonPoco = stuff.Value.ToObject<SeatJsonPoco>();

                    var seat = new Seat(seatJsonPoco.coach, int.Parse(seatJsonPoco.seat_number), seatJsonPoco.booking_reference);

                    if (!this.Coaches.ContainsKey(seat.CoachName))
                    {
                        this.Coaches[seat.CoachName] = new Coach(seat.CoachName);
                    }

                    var previousCoach = this.Coaches[seat.CoachName];
                    var newCoach = previousCoach.AddSeat(seat);

                    this.Coaches[seat.CoachName] = newCoach;
                }
            }
        }

        public int GetMaxSeat()
        {
            return this.Seats.Count;
        }

        public int ReservedSeats
        {
            get { return this.Seats.Count(s => s.BookingRef != string.Empty); }
        }

        public List<Seat> Seats => this.Coaches.Values.SelectMany(c => c.Seats).ToList();

        public bool DoesNotExceedOverallTrainCapacityLimit(int seatsRequestedCount)
        {
            return this.ReservedSeats + seatsRequestedCount <= Math.Floor(TrainMaxThresholdReservableSeats * this.Seats.Count);
        }

        public ReservationAttempt BuildReservationAttempt(int seatsRequestedCount)
        {
            var reservationAttempt = BuildReservationAttemptTheIdealCase(seatsRequestedCount);

            if (!reservationAttempt.IsFullfiled)
            {
                reservationAttempt = BuildReservationAttemptTheNonIdealCase(seatsRequestedCount, reservationAttempt);
            }

            return reservationAttempt;
        }

        private ReservationAttempt BuildReservationAttemptTheNonIdealCase(int seatsRequestedCount, ReservationAttempt reservationAttempt)
        {
            foreach (var coach in this.Coaches.Values)
            {
                reservationAttempt = coach.BuildReservationAttempt(seatsRequestedCount, this.trainId);
                if (reservationAttempt.IsFullfiled)
                {
                    break;
                }
            }

            return reservationAttempt;
        }

        private ReservationAttempt BuildReservationAttemptTheIdealCase(int seatsRequestedCount)
        {
            var reservationAttempt = new ReservationAttempt(seatsRequestedCount, this.trainId);

            foreach (var coach in this.Coaches.Values)
            {
                if (coach.DoesNotExceedOverallTrainCapacityLimit(seatsRequestedCount))
                {
                    reservationAttempt = coach.BuildReservationAttempt(seatsRequestedCount, this.trainId);
                    if (reservationAttempt.IsFullfiled)
                    {
                        break;
                    }
                }
            }

            return reservationAttempt;
        }

        protected override IEnumerable<object> GetAllAttributesToBeUsedForEquality()
        {
            return new List<object>() { new DictionaryByValue<string, Coach>(this.Coaches), new ListByValue<Seat>(this.Seats), this.ReservedSeats };
        }
    }
}