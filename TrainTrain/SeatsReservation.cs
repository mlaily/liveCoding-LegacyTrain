using System.Threading.Tasks;
using TrainTrain.Domain;
using TrainTrain.Infra;

namespace TrainTrain
{
    public class SeatsReservation : IReserveSeats
    {
        private readonly ITrainDataService _trainDataService;
        private readonly IBookingReferenceService _bookingReferenceService;

        public SeatsReservation(ITrainDataService trainDataService, IBookingReferenceService bookingReferenceService)
        {
            _bookingReferenceService = bookingReferenceService;
            _trainDataService = trainDataService;
        }

        public async Task<Reservation> Reserve(string trainId, int seatRequestedCount)
        {
            var train = await _trainDataService.GetTrain(trainId);

            if (train.DoesNotExceedOverallTrainCapacityThreashold(seatRequestedCount))
            {
                var reservationAttempt = train.BuildReservationAttempt(trainId, seatRequestedCount);

                if (reservationAttempt.IsFulfilled)
                {
                    var bookingReference = await _bookingReferenceService.GetBookingReference();

                    reservationAttempt.AssignBookingReference(bookingReference);
                    
                    await _trainDataService.Reserve(reservationAttempt);

                    return reservationAttempt.Confirm();
                }
            }
            return new ReservationFailure(trainId);
        }
    }
}