using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TrainTrain.Domain;

namespace TrainTrain.Infra
{
    public class ReserveSeatsRestAdapter
    {
        private readonly IReserveSeats hexagon;

        public ReserveSeatsRestAdapter(IReserveSeats hexagon)
        {
            this.hexagon = hexagon;
        }

        public async Task<string> Post(ReservationRequestDto reservationRequestDto)
        {
            // Mapping from infra to domain
            var trainId = reservationRequestDto.train_id;
            var seatsRequestedCount = reservationRequestDto.number_of_seats;

            // Call the domain logic
            var reservationAttempt = await this.hexagon.Reserve(trainId, seatsRequestedCount);

            // Map from domain to infra
            return JsonSerialize(reservationAttempt);
        }

        private static string JsonSerialize(ReservationAttempt reservationAttempt)
        {
            return $"{{\"train_id\": \"{reservationAttempt.TrainId}\", \"booking_reference\": \"{reservationAttempt.BookingReference}\", \"seats\": {dumpSeats(reservationAttempt.AvailableSeats)}}}";
        }

        private static string dumpSeats(IEnumerable<Seat> seats)
        {
            var sb = new StringBuilder("[");

            var firstTime = true;
            foreach (var seat in seats)
            {
                if (!firstTime)
                {
                    sb.Append(", ");
                }
                else
                {
                    firstTime = false;
                }

                sb.Append(String.Format("\"{0}{1}\"", seat.SeatNumber, seat.CoachName));
            }

            sb.Append("]");

            return sb.ToString();
        }
    }
}