using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using TrainTrain.Dal;
using TrainTrain.Dal.Entities;
using TrainTrain.Domain;
using TrainTrain.Infra;

namespace TrainTrain
{
    public interface IReserveSeats
    {
        Task<Reservation> Reserve(string trainId, int seatsRequestedCount);
    }

    public class WebTicketManager : IReserveSeats
    {
        private const string UriBookingReferenceService = "http://localhost:51691/";
        private const string UriTrainDataService = "http://localhost:50680";
        private readonly ITrainDataService _trainDataService;
        private readonly IBookingReferenceService _bookingReferenceService;

        public WebTicketManager():this(new TrainDataService(UriTrainDataService), new BookingReferenceService(UriBookingReferenceService))
        {
        }

        public WebTicketManager(ITrainDataService trainDataService, IBookingReferenceService bookingReferenceService)
        {
            _trainDataService = trainDataService;
            _bookingReferenceService = bookingReferenceService;
        }

        public async Task<Reservation> Reserve(string trainId, int seatsRequestedCount)
        {
            // get the train
            var train = await _trainDataService.GetTrain(trainId);

            if (train.DoNotExceedTrainCapacity(seatsRequestedCount))
            {
                var reservationAttempt = train.BuildReservationAttempt(seatsRequestedCount);

                if (reservationAttempt.IsFulfilled)
                {
                    var bookingRef = await _bookingReferenceService.GetBookingReference();

                    reservationAttempt.AssignBookingReference(bookingRef);

                    await _trainDataService.BookSeats(trainId, bookingRef, reservationAttempt.Seats);
                    return reservationAttempt.Confirm();

                }
            }

            return new ReservationFailure(trainId);
        }
    }
}