using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TrainTrain.Domain;
using TrainTrain.Infra;

namespace TrainTrain
{
    public class SeatsReservationAdapter
    {
        private IReserveSeats hexagon;

        public SeatsReservationAdapter(IReserveSeats hexagon)
        {
            this.hexagon = hexagon;
        }

        public static string AdaptReservation(Reservation reservation)
        {
            return
                $"{{\"train_id\": \"{reservation.TrainId}\", \"booking_reference\": \"{reservation.BookingRef}\", \"seats\": {DumpSeats(reservation.Seats)}}}";
        }

        private static string DumpSeats(IEnumerable<Seat> seats)
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

                sb.Append(string.Format("\"{0}{1}\"", seat.SeatNumber, seat.CoachName));
            }

            sb.Append("]");

            return sb.ToString();
        }

        public async Task<string> Post(ReservationRequestDto reservationRequestDto)
        {
            // Adapt from infra to domain
            var numberOfSeats = reservationRequestDto.number_of_seats;
            var trainId = reservationRequestDto.train_id;

            // Call business logic
            var reservation = await this.hexagon.Reserve(trainId, numberOfSeats);

            // Adapt from domain to infra
            return AdaptReservation(reservation);
        }
    }
}