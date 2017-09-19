using System.Collections.Generic;
using System.Text;

namespace TrainTrain
{
    public class ReservationAdapter
    {
        public static string AdaptReservation(Reservation reservation)
        {
            return
                $"{{\"train_id\": \"{reservation.TrainId}\", \"booking_reference\": \"{reservation.BookingRef}\", \"seats\": {DumpSeats(reservation.Seats)}}}";
        }

        public static string DumpSeats(IEnumerable<Seat> seats)
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
    }
}