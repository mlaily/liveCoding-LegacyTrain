using System.Threading.Tasks;

namespace TrainTrain.Domain
{
    // The hexagon
    public class SeatReservationService : IReserveSeats
    {
        private readonly ITrainDataService _trainDataService;
        private readonly IBookingReferenceService _bookingReferenceService;
        
        public SeatReservationService(ITrainDataService trainDataService, IBookingReferenceService bookingReferenceService)
        {
            _trainDataService = trainDataService;
            _bookingReferenceService = bookingReferenceService;
        }
        public async Task<Reservation> Reserve(string trainId, int seatsRequestedCount)
        {
            var train = await _trainDataService.GetTrain(trainId);
            if (train.DoesNotExceedTrainCapacityLimit(seatsRequestedCount))
            {
                var reservationAttempt = train.BuildReservationAttempt(trainId, seatsRequestedCount);

                if (reservationAttempt.IsFulfilled)
                {
                    var bookingRef = await _bookingReferenceService.GetBookingReference();

                    reservationAttempt.AssignBookingReference(bookingRef);
                    
                    await _trainDataService.BookSeats(reservationAttempt);

                    Reservation reservation = reservationAttempt.Confirm();

                    return (reservation);
                }
            }
            return (new ReservationFailed(trainId, seatsRequestedCount));
        }
    }
}